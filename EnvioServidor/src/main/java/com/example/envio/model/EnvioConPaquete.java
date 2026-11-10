package com.example.envio.model;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.time.LocalDateTime;

/**
 * Resultado de la consulta personalizada maestro-detalle: todos los datos del envio (maestro) mas
 * dos atributos de cada paquete (detalle): descripcion y peso. Si el envio no tiene paquetes,
 * los campos del detalle son null.
 */
@Data
@NoArgsConstructor
@AllArgsConstructor
public class EnvioConPaquete {
    private String codigo;
    private String origen;
    private String destino;
    private double precio;
    private LocalDateTime fechaSalida;
    private String departamentoDestino;
    private Integer codigoPostalNacional;
    private String paqueteCodigo;
    private String paqueteDescripcion;
    private Double paquetePesoKg;
}
