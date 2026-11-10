package com.example.envio.servicios;

import org.springframework.data.mongodb.core.query.Criteria;

import java.time.LocalDateTime;
import java.time.format.DateTimeParseException;
import java.util.regex.Pattern;

/** Utilidades compartidas por los servicios: criterios de busqueda y validaciones basicas. */
final class Consultas {
    private Consultas() {
    }

    static boolean vacio(String s) {
        return s == null || s.isBlank();
    }

    /** Coincidencia exacta ignorando mayusculas. */
    static Criteria igual(String campo, String valor) {
        return Criteria.where(campo).regex("^" + Pattern.quote(valor.trim()) + "$", "i");
    }

    /** El campo contiene el texto, ignorando mayusculas. */
    static Criteria contiene(String campo, String valor) {
        return Criteria.where(campo).regex(Pattern.quote(valor.trim()), "i");
    }

    static void requerido(String valor, String nombre) {
        if (vacio(valor)) {
            throw new ServicioException("El campo " + nombre + " es obligatorio.");
        }
    }

    static LocalDateTime fecha(String valor, String nombre) {
        requerido(valor, nombre);
        try {
            return LocalDateTime.parse(valor.trim());
        } catch (DateTimeParseException e) {
            throw new ServicioException("La fecha (" + nombre + ") debe tener formato yyyy-MM-ddTHH:mm:ss.");
        }
    }
}
