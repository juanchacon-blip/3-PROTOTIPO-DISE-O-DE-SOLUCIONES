package com.example.envio.controller;

import com.example.envio.model.Paquete;
import com.example.envio.model.PaqueteInput;
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

import java.time.LocalDateTime;
import java.time.format.DateTimeParseException;
import java.util.List;
import java.util.Map;
import java.util.Optional;

/**
 * Servicios web REST para paquetes (detalle de EnvioNacional).
 *
 *  GET    /api/paquetes                 -> lista (filtros opcionales: envioCodigo, descripcion)
 *  GET    /api/paquetes/buscar          -> consulta por 1 o 2 criterios (codigo, envioCodigo, descripcion)
 *  GET    /api/paquetes/{codigo}        -> consulta por codigo
 *  POST   /api/paquetes                 -> adiciona (el envio debe existir)
 *  PUT    /api/paquetes/{codigo}        -> actualiza
 *  DELETE /api/paquetes/{codigo}        -> elimina
 */
@RestController
@RequestMapping("/api/paquetes")
@CrossOrigin(origins = "http://localhost:3000")
public class PaqueteRestController {

    private final IServicioPaquete servicio;
    private final IServicioEnvioNacional servicioEnvios;

    public PaqueteRestController(IServicioPaquete servicio, IServicioEnvioNacional servicioEnvios) {
        this.servicio = servicio;
        this.servicioEnvios = servicioEnvios;
    }

    @GetMapping
    public List<Paquete> listar(
            @RequestParam(name = "envioCodigo", required = false) String envioCodigo,
            @RequestParam(name = "descripcion", required = false) String descripcion) {
        if (vacio(envioCodigo) && vacio(descripcion)) {
            return servicio.listarPaquetes();
        }
        return servicio.filtrarPaquetes(envioCodigo, descripcion);
    }

    @GetMapping("/buscar")
    public ResponseEntity<?> buscar(
            @RequestParam(name = "codigo", required = false) String codigo,
            @RequestParam(name = "envioCodigo", required = false) String envioCodigo,
            @RequestParam(name = "descripcion", required = false) String descripcion) {
        if (vacio(codigo) && vacio(envioCodigo) && vacio(descripcion)) {
            return error(HttpStatus.BAD_REQUEST, "Ingrese al menos un criterio de busqueda.");
        }
        Optional<Paquete> paquete = servicio.buscarPorCriterios(codigo, envioCodigo, descripcion);
        if (paquete.isEmpty()) {
            return error(HttpStatus.NOT_FOUND, "No se encontro ningun paquete con los criterios ingresados.");
        }
        return ResponseEntity.ok(paquete.get());
    }

    @GetMapping("/{codigo}")
    public ResponseEntity<?> porCodigo(@PathVariable("codigo") String codigo) {
        Optional<Paquete> paquete = servicio.buscarPorCodigo(codigo);
        if (paquete.isEmpty()) {
            return error(HttpStatus.NOT_FOUND, "No existe un paquete con ese codigo.");
        }
        return ResponseEntity.ok(paquete.get());
    }

    @PostMapping
    public ResponseEntity<?> adicionar(@RequestBody PaqueteInput input) {
        String problema = validar(input);
        if (problema != null) {
            return error(HttpStatus.BAD_REQUEST, problema);
        }
        if (servicioEnvios.buscarPorCodigo(input.envioCodigo()).isEmpty()) {
            return error(HttpStatus.NOT_FOUND, "El envio " + input.envioCodigo().trim() + " no existe.");
        }
        Optional<Paquete> creado = servicio.addPaquete(input);
        if (creado.isEmpty()) {
            return error(HttpStatus.CONFLICT, "Ya existe un paquete con ese codigo.");
        }
        return ResponseEntity.status(HttpStatus.CREATED).body(creado.get());
    }

    @PutMapping("/{codigo}")
    public ResponseEntity<?> actualizar(@PathVariable("codigo") String codigo, @RequestBody PaqueteInput input) {
        // El codigo de la URL manda sobre el del cuerpo.
        PaqueteInput datos = new PaqueteInput(codigo, input.envioCodigo(), input.descripcion(), input.pesoKg(),
                input.unidades(), input.fechaRegistro(), input.fragil());
        String problema = validar(datos);
        if (problema != null) {
            return error(HttpStatus.BAD_REQUEST, problema);
        }
        Optional<Paquete> actualizado = servicio.updatePaquete(datos);
        if (actualizado.isEmpty()) {
            return error(HttpStatus.NOT_FOUND, "El paquete ya no existe.");
        }
        return ResponseEntity.ok(actualizado.get());
    }

    @DeleteMapping("/{codigo}")
    public ResponseEntity<?> eliminar(@PathVariable("codigo") String codigo) {
        Optional<Paquete> eliminado = servicio.deletePaquete(codigo);
        if (eliminado.isEmpty()) {
            return error(HttpStatus.NOT_FOUND, "El paquete ya no existe.");
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
    private static String validar(PaqueteInput in) {
        if (in == null || vacio(in.codigo()) || vacio(in.envioCodigo()) || vacio(in.descripcion())
                || vacio(in.fechaRegistro())) {
            return "Complete todos los campos.";
        }
        if (in.pesoKg() <= 0) {
            return "El peso debe ser mayor que cero.";
        }
        if (in.unidades() < 1) {
            return "Las unidades deben ser al menos 1.";
        }
        try {
            LocalDateTime.parse(in.fechaRegistro());
        } catch (DateTimeParseException e) {
            return "La fecha debe tener formato yyyy-MM-ddTHH:mm:ss.";
        }
        return null;
    }
}
