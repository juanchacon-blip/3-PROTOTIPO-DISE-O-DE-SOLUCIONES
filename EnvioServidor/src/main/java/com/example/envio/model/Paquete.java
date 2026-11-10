package com.example.envio.model;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.Transient;
import org.springframework.data.mongodb.core.mapping.Document;

import java.time.LocalDateTime;

/**
 * DETALLE de la relacion maestro-detalle. Se guarda en la coleccion "paquetes" de MongoDB y
 * referencia a su EnvioNacional por envioCodigo (0..* paquetes por envio).
 * Atributos: String (codigo = llave primaria, envioCodigo, descripcion), double (pesoKg), int (unidades),
 * LocalDateTime (fechaRegistro) y boolean (fragil, opcional: por defecto false).
 * envioOrigen y envioDestino NO se guardan: el servicio los completa con datos del maestro al consultar.
 */
@Data
@NoArgsConstructor
@AllArgsConstructor
@Document(collection = "paquetes")
public class Paquete {
    @Id
    private String codigo;
    private String envioCodigo;
    private String descripcion;
    private double pesoKg;
    private int unidades;
    private LocalDateTime fechaRegistro;
    private boolean fragil;

    @Transient
    private String envioOrigen;
    @Transient
    private String envioDestino;

    public Paquete(String codigo, String envioCodigo, String descripcion, double pesoKg, int unidades,
                   LocalDateTime fechaRegistro, boolean fragil) {
        this(codigo, envioCodigo, descripcion, pesoKg, unidades, fechaRegistro, fragil, null, null);
    }
}
