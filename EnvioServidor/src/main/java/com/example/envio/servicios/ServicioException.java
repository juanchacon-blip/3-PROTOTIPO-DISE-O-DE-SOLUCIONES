package com.example.envio.servicios;

/** Error de negocio con un mensaje apto para mostrar al usuario del cliente. */
public class ServicioException extends RuntimeException {
    public ServicioException(String mensaje) {
        super(mensaje);
    }
}
