package com.example.envio.servicios;

import com.example.envio.model.EnvioNacional;
import com.example.envio.model.EnvioNacionalInput;

import java.util.List;
import java.util.Optional;

/**
 * Contrato del servicio de envios nacionales. Los controladores (GraphQL y REST) dependen de
 * esta interfaz y no de la clase concreta (principio de inversion de dependencias, SOLID).
 */
public interface IServicioEnvioNacional {

    List<EnvioNacional> listarEnvios();

    List<EnvioNacional> filtrarEnvios(String origen, String departamentoDestino);

    Optional<EnvioNacional> buscarPorCodigo(String codigo);

    Optional<EnvioNacional> buscarPorCriterios(String codigo, String origen, String destino);

    Optional<EnvioNacional> addEnvio(EnvioNacionalInput input);

    Optional<EnvioNacional> updateEnvio(EnvioNacionalInput input);

    Optional<EnvioNacional> deleteEnvio(String codigo);
}
