package com.example.envio.controller;

import com.example.envio.model.Paquete;
import com.example.envio.model.PaqueteInput;
import com.example.envio.model.EnvioConPaquete;
import com.example.envio.servicios.IServicioPaquete;
import com.example.envio.servicios.ServicioException;
import org.springframework.graphql.data.method.annotation.Argument;
import org.springframework.graphql.data.method.annotation.MutationMapping;
import org.springframework.graphql.data.method.annotation.QueryMapping;
import org.springframework.stereotype.Controller;

import java.util.List;
import java.util.Optional;

@Controller
public class PaqueteController {

    private final IServicioPaquete servicio;

    public PaqueteController(IServicioPaquete servicio) {
        this.servicio = servicio;
    }

    @QueryMapping
    public List<Paquete> paquetes() {
        return servicio.listarPaquetes();
    }

    @QueryMapping
    public List<Paquete> paquetesFiltrados(@Argument String envioCodigo, @Argument String descripcion) {
        return servicio.filtrarPaquetes(envioCodigo, descripcion);
    }

    @QueryMapping
    public Optional<Paquete> paquetePorCodigo(@Argument String codigo) {
        return servicio.buscarPorCodigo(codigo);
    }

    @QueryMapping
    public Optional<Paquete> paquetePorCriterios(@Argument String codigo, @Argument String envioCodigo,
                                                 @Argument String descripcion) {
        return servicio.buscarPorCriterios(codigo, envioCodigo, descripcion);
    }

    @MutationMapping
    public Optional<Paquete> addPaquete(@Argument("input") PaqueteInput input) {
        Optional<Paquete> creado = servicio.addPaquete(input);
        if (creado.isEmpty()) {
            throw new ServicioException("Ya existe un paquete con ese codigo.");
        }
        return creado;
    }

    @MutationMapping
    public Optional<Paquete> updatePaquete(@Argument("input") PaqueteInput input) {
        Optional<Paquete> actualizado = servicio.updatePaquete(input);
        if (actualizado.isEmpty()) {
            throw new ServicioException("El paquete ya no existe.");
        }
        return actualizado;
    }

    @MutationMapping
    public Optional<Paquete> deletePaquete(@Argument String codigo) {
        Optional<Paquete> eliminado = servicio.deletePaquete(codigo);
        if (eliminado.isEmpty()) {
            throw new ServicioException("El paquete ya no existe.");
        }
        return eliminado;
    }

    /** Consulta personalizada maestro-detalle: datos del envio mas descripcion y peso de sus paquetes. */
    @QueryMapping
    public List<EnvioConPaquete> enviosConPaquetes(@Argument String envioCodigo) {
        return servicio.enviosConPaquetes(envioCodigo);
    }
}
