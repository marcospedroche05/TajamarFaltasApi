Eres el Orquestador del proyecto TajamarFaltas API. Lee CLAUDE.md para conocer tu rol, los agentes disponibles y el flujo de trabajo.

## Objetivo

Determinar la siguiente tarea a implementar y presentar un plan de ejecucion al usuario para su aprobacion.

## Pasos

1. **Lee el estado actual del proyecto:**
   - `docs/10-pendientes.md` — tareas pendientes priorizadas.
   - `docs/04-modelo-de-datos.md` — modelo de datos actual.
   - `docs/05-endpoints.md` — endpoints existentes.
   - `docs/07-autenticacion.md` — flujo de auth actual.

2. **Analiza el codigo para detectar discrepancias:**
   - Compara el DbContext (`backend/src/TajamarFaltas.Infrastructure/Persistence/TajamarDbContext.cs`) con el modelo de datos documentado.
   - Verifica si hay entidades, servicios o endpoints a medio implementar.
   - Comprueba si hay TODOs o codigo placeholder (archivos `Class1.cs`, `UnitTest1.cs`).

3. **Prioriza la siguiente tarea** considerando:
   - Dependencias: que tareas desbloquean otras.
   - Impacto: que aporta mas valor al frontend Angular.
   - Riesgo: que puede romper lo existente.

4. **Presenta al usuario:**

```
## Estado actual
[Resumen en 2-3 lineas de lo que esta hecho y funcional]

## Tarea recomendada
[Nombre de la tarea]

### Por que esta primero
[1-2 frases sobre dependencias o impacto]

### Plan de ejecucion
| Paso | Agente | Accion | Archivos afectados |
|------|--------|--------|--------------------|
| 1    | ...    | ...    | ...                |
| 2    | ...    | ...    | ...                |
| ...  | ...    | ...    | ...                |

### Alternativas
- [Otra tarea posible y por que no va primero]

## ¿Procedo con esta tarea o prefieres otra?
```

## Reglas

- No implementes nada todavia. Solo analiza y propone.
- Si encuentras incoherencias entre docs y codigo, mencionalo.
- Si hay varias tareas con la misma prioridad, pregunta al usuario.
