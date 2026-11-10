package com.example.envio.servicios;

import com.example.envio.model.EnvioConPaquete;
import com.example.envio.model.Paquete;
import com.example.envio.model.PaqueteInput;

import java.util.List;
import java.util.Optional;

/** Contrato del servicio de paquetes (detalle). Los controladores dependen de esta interfaz (SOLID: DIP). */
public interface IServicioPaquete {

    /** Todos los paquetes, con origen y destino de su envio (envioOrigen, envioDestino). */
    List<Paquete> listarPaquetes();

    List<Paquete> filtrarPaquetes(String envioCodigo, String descripcion);

    Optional<Paquete> buscarPorCodigo(String codigo);

    Optional<Paquete> buscarPorCriterios(String codigo, String envioCodigo, String descripcion);

    /** Vacio si el codigo ya existe; lanza ServicioException si el envio (maestro) no existe o hay datos invalidos. */
    Optional<Paquete> addPaquete(PaqueteInput input);

    /** Actualiza los datos del paquete; el envio al que pertenece no cambia. */
    Optional<Paquete> updatePaquete(PaqueteInput input);

    Optional<Paquete> deletePaquete(String codigo);

    /** Consulta maestro-detalle: datos del envio mas descripcion y peso de cada uno de sus paquetes. */
    List<EnvioConPaquete> enviosConPaquetes(String envioCodigo);
}
