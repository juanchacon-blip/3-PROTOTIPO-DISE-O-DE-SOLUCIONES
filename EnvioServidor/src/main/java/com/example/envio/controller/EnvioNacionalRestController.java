package com.example.envio.controller;

import com.example.envio.model.EnvioNacional;
import com.example.envio.model.EnvioNacionalInput;
import com.example.envio.model.EnvioConPaquete;
import com.example.envio.servicios.IServicioEnvioNacional;
import com.example.envio.servicios.IServicioPaquete;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.time.format.DateTimeParseException;
import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;
import java.util.Optional;

/**
 * Servicios web REST para envios nacionales (usa el mismo servicio Singleton que GraphQL, inyectado por interfaz).
 *
 *  GET    /api/envios-nacionales                      -> lista (filtros opcionales: origen, departamentoDestino)
 *  GET    /api/envios-nacionales/con-paquetes         -> envio + descripcion y peso de sus paquetes
 *  GET    /api/envios-nacionales/buscar               -> consulta por 1 o 2 criterios (codigo, origen, destino)
 *  GET    /api/envios-nacionales/{codigo}             -> consulta por codigo
 *  POST   /api/envios-nacionales                      -> adiciona
 *  PUT    /api/envios-nacionales/{codigo}             -> actualiza
 *  DELETE /api/envios-nacionales/{codigo}             -> elimina
 */
@RestController
@RequestMapping("/api/envios-nacionales")
@CrossOrigin(origins = "http://localhost:3000")
public class EnvioNacionalRestController {

    private final IServicioEnvioNacional servicio;
    private final IServicioPaquete servicioPaquetes;

    public EnvioNacionalRestController(IServicioEnvioNacional servicio, IServicioPaquete servicioPaquetes) {
        this.servicio = servicio;
        this.servicioPaquetes = servicioPaquetes;
    }

    /** Consulta maestro-detalle: datos del envio mas descripcion y peso de sus paquetes (filtro opcional por envio). */
    @GetMapping("/con-paquetes")
    public List<EnvioConPaquete> conPaquetes(
            @RequestParam(name = "envioCodigo", required = false) String envioCodigo) {
        return servicioPaquetes.enviosConPaquetes(envioCodigo);
    }

    @GetMapping
    public List<EnvioNacional> listar(
            @RequestParam(name = "origen", required = false) String origen,
            @RequestParam(name = "departamentoDestino", required = false) String departamentoDestino) {
        if (vacio(origen) && vacio(departamentoDestino)) {
            return servicio.listarEnvios();
        }
        return servicio.filtrarEnvios(origen, departamentoDestino);
    }

    @GetMapping("/buscar")
    public ResponseEntity<?> buscar(
            @RequestParam(name = "codigo", required = false) String codigo,
            @RequestParam(name = "origen", required = false) String origen,
            @RequestParam(name = "destino", required = false) String destino) {
        if (vacio(codigo) && vacio(origen) && vacio(destino)) {
            return error(HttpStatus.BAD_REQUEST, "Ingrese al menos un criterio de busqueda.");
        }
        Optional<EnvioNacional> envio = servicio.buscarPorCriterios(codigo, origen, destino);
        if (envio.isEmpty()) {
            return error(HttpStatus.NOT_FOUND, "No se encontro ningun envio con los criterios ingresados.");
        }
        return ResponseEntity.ok(envio.get());
    }

    @GetMapping("/{codigo}")
    public ResponseEntity<?> porCodigo(@PathVariable("codigo") String codigo) {
        Optional<EnvioNacional> envio = servicio.buscarPorCodigo(codigo);
        if (envio.isEmpty()) {
            return error(HttpStatus.NOT_FOUND, "No existe un envio con ese codigo.");
        }
        return ResponseEntity.ok(envio.get());
    }

    @PostMapping
    public ResponseEntity<?> adicionar(@RequestBody EnvioNacionalInput input) {
        String problema = validar(input);
        if (problema != null) {
            return error(HttpStatus.BAD_REQUEST, problema);
        }
        Optional<EnvioNacional> creado = servicio.addEnvio(input);
        if (creado.isEmpty()) {
            return error(HttpStatus.CONFLICT, "Ya existe un envio con ese codigo.");
        }
        return ResponseEntity.status(HttpStatus.CREATED).body(creado.get());
    }

    @PutMapping("/{codigo}")
    public ResponseEntity<?> actualizar(@PathVariable("codigo") String codigo,
                                        @RequestBody EnvioNacionalInput input) {
        // El codigo de la URL manda sobre el del cuerpo.
        EnvioNacionalInput datos = new EnvioNacionalInput(codigo, input.origen(), input.destino(),
                input.precio(), input.fechaSalida(), input.departamentoDestino(), input.codigoPostalNacional());
        String problema = validar(datos);
        if (problema != null) {
            return error(HttpStatus.BAD_REQUEST, problema);
        }
        Optional<EnvioNacional> actualizado = servicio.updateEnvio(datos);
        if (actualizado.isEmpty()) {
            return error(HttpStatus.NOT_FOUND, "El envio ya no existe.");
        }
        return ResponseEntity.ok(actualizado.get());
    }

    @DeleteMapping("/{codigo}")
    public ResponseEntity<?> eliminar(@PathVariable("codigo") String codigo) {
        Optional<EnvioNacional> eliminado = servicio.deleteEnvio(codigo);
        if (eliminado.isEmpty()) {
            return error(HttpStatus.NOT_FOUND, "El envio ya no existe.");
        }
        return ResponseEntity.ok(eliminado.get());
    }

    private static boolean vacio(String s) {
        return s == null || s.isBlank();
    }

    private static ResponseEntity<Map<String, String>> error(HttpStatus estado, String mensaje) {
        return ResponseEntity.status(estado).body(Map.of("mensaje", mensaje));
    }

    /** Devuelve el mensaje del problema, o null si los datos son validos. */
    private static String validar(EnvioNacionalInput in) {
        if (in == null || vacio(in.codigo()) || vacio(in.origen()) || vacio(in.destino())
                || vacio(in.departamentoDestino()) || vacio(in.fechaSalida())) {
            return "Complete todos los campos.";
        }
        if (in.precio() < 0) {
            return "El precio no puede ser negativo.";
        }
        try {
            LocalDateTime.parse(in.fechaSalida());
        } catch (DateTimeParseException e) {
            return "La fecha debe tener formato yyyy-MM-ddTHH:mm:ss.";
        }
        return null;
    }
}
