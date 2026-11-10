package com.example.envio.servicios;

import com.example.envio.model.EnvioNacional;
import com.example.envio.model.EnvioNacionalInput;
import com.example.envio.model.Paquete;
import org.springframework.data.mongodb.core.MongoTemplate;
import org.springframework.data.mongodb.core.query.Criteria;
import org.springframework.data.mongodb.core.query.Query;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

import static com.example.envio.servicios.Consultas.igual;
import static com.example.envio.servicios.Consultas.requerido;
import static com.example.envio.servicios.Consultas.vacio;

/** Gestiona los envios nacionales (maestro) en la coleccion "envios" de MongoDB. Bean singleton de Spring. */
@Service
public class ServicioEnvioNacional implements IServicioEnvioNacional {

    private final MongoTemplate mongo;

    public ServicioEnvioNacional(MongoTemplate mongo) {
        this.mongo = mongo;
    }

    @Override
    public List<EnvioNacional> listarEnvios() {
        return mongo.findAll(EnvioNacional.class);
    }

    @Override
    public List<EnvioNacional> filtrarEnvios(String origen, String departamentoDestino) {
        List<Criteria> filtros = new ArrayList<>();
        if (!vacio(origen)) {
            filtros.add(igual("origen", origen));
        }
        if (!vacio(departamentoDestino)) {
            filtros.add(igual("departamentoDestino", departamentoDestino));
        }
        return mongo.find(consulta(filtros), EnvioNacional.class);
    }

    @Override
    public Optional<EnvioNacional> buscarPorCodigo(String codigo) {
        if (vacio(codigo)) {
            return Optional.empty();
        }
        return Optional.ofNullable(mongo.findById(codigo.trim(), EnvioNacional.class));
    }

    @Override
    public Optional<EnvioNacional> buscarPorCriterios(String codigo, String origen, String destino) {
        List<Criteria> filtros = new ArrayList<>();
        if (!vacio(codigo)) {
            filtros.add(Criteria.where("_id").is(codigo.trim()));
        }
        if (!vacio(origen)) {
            filtros.add(igual("origen", origen));
        }
        if (!vacio(destino)) {
            filtros.add(igual("destino", destino));
        }
        if (filtros.isEmpty()) {
            return Optional.empty();
        }
        return Optional.ofNullable(mongo.findOne(consulta(filtros), EnvioNacional.class));
    }

    @Override
    public synchronized Optional<EnvioNacional> addEnvio(EnvioNacionalInput input) {
        EnvioNacional envio = construir(input);
        if (mongo.exists(Query.query(Criteria.where("_id").is(envio.getCodigo())), EnvioNacional.class)) {
            return Optional.empty();
        }
        return Optional.of(mongo.insert(envio));
    }

    @Override
    public synchronized Optional<EnvioNacional> updateEnvio(EnvioNacionalInput input) {
        EnvioNacional nuevo = construir(input);
        if (!mongo.exists(Query.query(Criteria.where("_id").is(nuevo.getCodigo())), EnvioNacional.class)) {
            return Optional.empty();
        }
        return Optional.of(mongo.save(nuevo));
    }

    /** Elimina el envio y, en cascada, sus paquetes. */
    @Override
    public synchronized Optional<EnvioNacional> deleteEnvio(String codigo) {
        Optional<EnvioNacional> envio = buscarPorCodigo(codigo);
        envio.ifPresent(e -> {
            mongo.remove(Query.query(Criteria.where("envioCodigo").is(e.getCodigo())), Paquete.class);
            mongo.remove(e);
        });
        return envio;
    }

    private static Query consulta(List<Criteria> filtros) {
        Query query = new Query();
        if (!filtros.isEmpty()) {
            query.addCriteria(new Criteria().andOperator(filtros));
        }
        return query;
    }

    /** Valida los datos de entrada y construye el envio; lanza ServicioException si algo es invalido. */
    private static EnvioNacional construir(EnvioNacionalInput in) {
        if (in == null) {
            throw new ServicioException("Complete los datos del envio.");
        }
        requerido(in.codigo(), "codigo");
        requerido(in.origen(), "origen");
        requerido(in.destino(), "destino");
        requerido(in.departamentoDestino(), "departamento de destino");
        if (!(in.precio() > 0)) {
            throw new ServicioException("El precio debe ser mayor que cero.");
        }
        LocalDateTime fecha = Consultas.fecha(in.fechaSalida(), "fecha de salida");
        Integer postal = in.codigoPostalNacional();
        if (postal != null && (postal < 1 || postal > 999999)) {
            throw new ServicioException("El codigo postal debe ser un numero positivo de hasta 6 digitos.");
        }
        return new EnvioNacional(in.codigo().trim(), in.origen().trim(), in.destino().trim(), in.precio(),
                fecha, in.departamentoDestino().trim(), postal);
    }
}
