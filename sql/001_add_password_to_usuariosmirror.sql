-- =============================================================================
-- Script : 001_add_password_to_usuariosmirror.sql
-- Proposito: Agregar la columna Password a la tabla UsuariosMirror.
--            La columna almacena la contrasena en texto plano (entorno de test).
--            Todos los registros existentes reciben el valor por defecto '12345'.
-- Idempotente: se puede ejecutar multiples veces sin error.
-- =============================================================================

