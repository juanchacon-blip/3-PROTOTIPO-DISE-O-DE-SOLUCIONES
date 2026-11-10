package com.example.envio.model;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import org.springframework.data.annotation.Id;

import java.time.LocalDateTime;

/** Clase base de los envios. Atributos: String (codigo = llave primaria, origen, destino), double, LocalDateTime. */
@Data
@AllArgsConstructor
@NoArgsConstructor
public class Envio {
    @Id
    private String codigo;
    private String origen;
    private String destino;
    private double precio;
    private LocalDateTime fechaSalida;
}
