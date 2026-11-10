package com.example.envio.controller;

import com.example.envio.servicios.ServicioException;
import graphql.GraphQLError;
import graphql.GraphqlErrorBuilder;
import graphql.schema.DataFetchingEnvironment;
import org.springframework.graphql.execution.DataFetcherExceptionResolverAdapter;
import org.springframework.graphql.execution.ErrorType;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;
import org.springframework.stereotype.Component;

import java.util.Map;

/**
 * Manejo centralizado de excepciones de negocio (ServicioException): los clientes reciben un
 * mensaje claro tanto por GraphQL (campo "errors") como por REST ({"mensaje": "..."} con HTTP 400).
 */
public class ManejadorErrores {

    @Component
    public static class GraphQL extends DataFetcherExceptionResolverAdapter {
        @Override
        protected GraphQLError resolveToSingleError(Throwable ex, DataFetchingEnvironment env) {
            if (ex instanceof ServicioException) {
                return GraphqlErrorBuilder.newError(env)
                        .errorType(ErrorType.BAD_REQUEST)
                        .message(ex.getMessage())
                        .build();
            }
            return null;
        }
    }

    @RestControllerAdvice
    public static class Rest {
        @ExceptionHandler(ServicioException.class)
        public ResponseEntity<Map<String, String>> servicio(ServicioException ex) {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(Map.of("mensaje", ex.getMessage()));
        }
    }
}
