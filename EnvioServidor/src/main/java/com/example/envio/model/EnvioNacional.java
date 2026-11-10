package com.example.envio.model;

import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;
import org.springframework.data.mongodb.core.mapping.Document;

import java.time.LocalDateTime;

/**
 * MAESTRO de la relacion maestro-detalle. Se guarda en la coleccion "envios" de MongoDB.
 * No contiene listas: los paquetes (detalle) viven en su propia coleccion y apuntan al envio
 * con Paquete.envioCodigo (Figura 2 del enunciado).
 * Obligatorios: codigo, origen, destino, precio, fechaSalida, departamentoDestino.
 * Opcional: codigoPostalNacional.
 */
@Data
@NoArgsConstructor
@EqualsAndHashCode(callSuper = true)
@Document(collection = "envios")
public class EnvioNacional extends Envio {
    private String departamentoDestino;
    private Integer codigoPostalNacional; // opcional

    public EnvioNacional(String codigo, String origen, String destino, double precio,
                         LocalDateTime fechaSalida, String departamentoDestino, Integer codigoPostalNacional) {
        super(codigo, origen, destino, precio, fechaSalida);
        this.departamentoDestino = departamentoDestino;
        this.codigoPostalNacional = codigoPostalNacional;
    }
}
