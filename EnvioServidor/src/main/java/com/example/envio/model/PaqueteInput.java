package com.example.envio.model;

/** fragil es opcional (si es null se toma como false). */
public record PaqueteInput(String codigo, String envioCodigo, String descripcion, double pesoKg,
                           int unidades, String fechaRegistro, Boolean fragil) {
}
