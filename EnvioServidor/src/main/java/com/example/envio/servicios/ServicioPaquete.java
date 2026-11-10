package com.example.envio.servicios;

import com.example.envio.model.EnvioConPaquete;
import com.example.envio.model.EnvioNacional;
import com.example.envio.model.Paquete;
import com.example.envio.model.PaqueteInput;
import org.springframework.data.mongodb.core.MongoTemplate;
import org.springframework.data.mongodb.core.query.Criteria;
import org.springframework.data.mongodb.core.query.Query;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Optional;

import static com.example.envio.servicios.Consultas.contiene;
import static com.example.envio.servicios.Consultas.igual;
import static com.example.envio.servicios.Consultas.requerido;
import static com.example.envio.servicios.Consultas.vacio;

/**
 * Gestiona los paquetes (detalle) en la coleccion "paquetes" de MongoDB. Cada paquete referencia a su
 * envio por envioCodigo; el maestro se obtiene a traves de IServicioEnvioNacional (no de la clase concreta).
 */
@Service
public class ServicioPaquete implements IServicioPaquete {

    private final MongoTemplate mongo;
    private final IServicioEnvioNacional servicioEnvios;

    public ServicioPaquete(MongoTemplate mongo, IServicioEnvioNacional servicioEnvios) {
        this.mongo = mongo;
        this.servicioEnvios = servicioEnvios;
    }

    @Override
    public List<Paquete> listarPaquetes() {
        return completar(mongo.findAll(Paquete.class));
    }

    @Override
    public List<Paquete> filtrarPaquetes(String envioCodigo, String descripcion) {
        return completar(mongo.find(consulta(envioCodigo, descripcion, null), Paquete.class));
    }

    @Override
    public Optional<Paquete> buscarPorCodigo(String codigo) {
        if (vacio(codigo)) {
            return Optional.empty();
        }
        Paquete p = mongo.findById(codigo.trim(), Paquete.class);
        return p == null ? Optional.empty() : Optional.of(completar(List.of(p)).get(0));
    }

    @Override
    public Optional<Paquete> buscarPorCriterios(String codigo, String envioCodigo, String descripcion) {
        if (vacio(codigo) && vacio(envioCodigo) && vacio(descripcion)) {
            return Optional.empty();
        }
        Paquete p = mongo.findOne(consulta(envioCodigo, descripcion, codigo), Paquete.class);
        return p == null ? Optional.empty() : Optional.of(completar(List.of(p)).get(0));
    }

    @Override
    public synchronized Optional<Paquete> addPaquete(PaqueteInput input) {
        Paquete nuevo = construir(input);
        if (servicioEnvios.buscarPorCodigo(nuevo.getEnvioCodigo()).isEmpty()) {
            throw new ServicioException("El envio " + nuevo.getEnvioCodigo() + " no existe.");
        }
        if (mongo.exists(Query.query(Criteria.where("_id").is(nuevo.getCodigo())), Paquete.class)) {
            return Optional.empty();
        }
        return Optional.of(completar(List.of(mongo.insert(nuevo))).get(0));
    }

    @Override
    public synchronized Optional<Paquete> updatePaquete(PaqueteInput input) {
        Paquete nuevo = construir(input);
        Paquete actual = mongo.findById(nuevo.getCodigo(), Paquete.class);
        if (actual == null) {
            return Optional.empty();
        }
        // El envio al que pertenece el paquete no cambia al actualizarlo.
        nuevo.setEnvioCodigo(actual.getEnvioCodigo());
        return Optional.of(completar(List.of(mongo.save(nuevo))).get(0));
    }

    @Override
    public synchronized Optional<Paquete> deletePaquete(String codigo) {
        Optional<Paquete> paquete = buscarPorCodigo(codigo);
        paquete.ifPresent(mongo::remove);
        return paquete;
    }

    /** Une cada envio con sus paquetes (left join): todos los datos del envio + descripcion y peso del paquete. */
    @Override
    public List<EnvioConPaquete> enviosConPaquetes(String envioCodigo) {
        List<EnvioNacional> envios = new ArrayList<>();
        if (vacio(envioCodigo)) {
            envios.addAll(servicioEnvios.listarEnvios());
        } else {
            servicioEnvios.buscarPorCodigo(envioCodigo).ifPresent(envios::add);
        }
        List<EnvioConPaquete> filas = new ArrayList<>();
        for (EnvioNacional e : envios) {
            List<Paquete> paquetes = mongo.find(
                    Query.query(Criteria.where("envioCodigo").is(e.getCodigo())), Paquete.class);
            if (paquetes.isEmpty()) {
                filas.add(fila(e, null));
            }
            for (Paquete p : paquetes) {
                filas.add(fila(e, p));
            }
        }
        return filas;
    }

    private static EnvioConPaquete fila(EnvioNacional e, Paquete p) {
        return new EnvioConPaquete(e.getCodigo(), e.getOrigen(), e.getDestino(), e.getPrecio(),
                e.getFechaSalida(), e.getDepartamentoDestino(), e.getCodigoPostalNacional(),
                p == null ? null : p.getCodigo(), p == null ? null : p.getDescripcion(),
                p == null ? null : p.getPesoKg());
    }

    private static Query consulta(String envioCodigo, String descripcion, String codigo) {
        List<Criteria> filtros = new ArrayList<>();
        if (!vacio(codigo)) {
            filtros.add(Criteria.where("_id").is(codigo.trim()));
        }
        if (!vacio(envioCodigo)) {
            filtros.add(igual("envioCodigo", envioCodigo));
        }
        if (!vacio(descripcion)) {
            filtros.add(contiene("descripcion", descripcion));
        }
        Query query = new Query();
        if (!filtros.isEmpty()) {
            query.addCriteria(new Criteria().andOperator(filtros));
        }
        return query;
    }

    /** Completa envioOrigen y envioDestino (datos del maestro) de cada paquete con una sola consulta. */
    private List<Paquete> completar(List<Paquete> paquetes) {
        Map<String, EnvioNacional> envios = new HashMap<>();
        for (Paquete p : paquetes) {
            if (!envios.containsKey(p.getEnvioCodigo())) {
                envios.put(p.getEnvioCodigo(), servicioEnvios.buscarPorCodigo(p.getEnvioCodigo()).orElse(null));
            }
            EnvioNacional e = envios.get(p.getEnvioCodigo());
            if (e != null) {
                p.setEnvioOrigen(e.getOrigen());
                p.setEnvioDestino(e.getDestino());
            }
        }
        return paquetes;
    }

    /** Valida los datos de entrada y construye el paquete; lanza ServicioException si algo es invalido. */
    private static Paquete construir(PaqueteInput in) {
        if (in == null) {
            throw new ServicioException("Complete los datos del paquete.");
        }
        requerido(in.codigo(), "codigo");
        requerido(in.envioCodigo(), "codigo del envio");
        requerido(in.descripcion(), "descripcion");
        if (!(in.pesoKg() > 0)) {
            throw new ServicioException("El peso debe ser mayor que cero.");
        }
        if (in.unidades() < 1) {
            throw new ServicioException("Las unidades deben ser un numero entero mayor o igual a 1.");
        }
        return new Paquete(in.codigo().trim(), in.envioCodigo().trim(), in.descripcion().trim(), in.pesoKg(),
                in.unidades(), Consultas.fecha(in.fechaRegistro(), "fecha de registro"),
                Boolean.TRUE.equals(in.fragil()));
    }
}
