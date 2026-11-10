package com.example.envio.model;

/** codigoPostalNacional es opcional (puede ser null). */
public record EnvioNacionalInput(String codigo, String origen, String destino, double precio,
                                 String fechaSalida, String departamentoDestino, Integer codigoPostalNacional) {
}
