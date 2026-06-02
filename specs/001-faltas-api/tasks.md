# Tasks: API de Gestión de Faltas Tajamar

**Input**: Design documents from `/specs/001-faltas-api/`

**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

**Tests**: Tests are not explicitly requested in the specification, so the list below focuses on implementation tasks and the minimum structural work needed to deliver each user story independently.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and base backend structure

- [X] T001 Create the backend solution structure and root folders for `backend/src/TajamarFaltas.Api`, `backend/src/TajamarFaltas.Application`, `backend/src/TajamarFaltas.Domain`, `backend/src/TajamarFaltas.Infrastructure`, and `backend/tests/`
- [X] T002 Create the API project and install core packages in `backend/src/TajamarFaltas.Api/TajamarFaltas.Api.csproj` for ASP.NET Core Web API, OpenAPI, JWT bearer auth, Entity Framework Core, SQL Server, and `Scalar.AspNetCore`
- [X] T003 Create the application, domain, and infrastructure class library projects under `backend/src/` and wire project references between `TajamarFaltas.Api`, `TajamarFaltas.Application`, `TajamarFaltas.Domain`, and `TajamarFaltas.Infrastructure`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core infrastructure that must exist before any user story can be implemented

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

- [X] T004 Define the EF Core database context and entity mappings for `RolesMirror`, `UsuariosMirror`, `CursosMirror`, and `Faltas` in `backend/src/TajamarFaltas.Infrastructure/Persistence/`
- [X] T005 Create the domain entities and enums for roles, users, courses, and absence types in `backend/src/TajamarFaltas.Domain/Entities/` and `backend/src/TajamarFaltas.Domain/Enums/`
- [X] T006 Implement dependency injection registration for persistence, authentication, and external services in `backend/src/TajamarFaltas.Infrastructure/DependencyInjection/`
- [X] T007 Configure centralized app settings and environment binding for SQL Server, JWT, and Tajamar external API credentials in `backend/src/TajamarFaltas.Api/appsettings*.json` and `backend/src/TajamarFaltas.Api/Program.cs`
- [X] T008 Implement the `TajamarApiClientService` with admin login, JWT caching, bearer header injection, and API base URL handling in `backend/src/TajamarFaltas.Infrastructure/ExternalServices/TajamarApiClientService.cs`
- [X] T009 Configure OpenAPI generation and Scalar UI mapping in `backend/src/TajamarFaltas.Api/Program.cs` so the interactive explorer is available at `/scalar` and can send Bearer tokens on protected requests
- [X] T010 Add shared authorization policies and role-claim helpers for Alumno, Profesor, and Administrador in `backend/src/TajamarFaltas.Api/Authorization/`

**Checkpoint**: Foundation ready - user story implementation can now begin in parallel

---

## Phase 3: User Story 1 - Autenticación y acceso inicial (Priority: P1) 🎯 MVP

**Goal**: Allow a Tajamar user to authenticate locally, receive a JWT, and enter the system with role-based claims.

**Independent Test**: A valid Tajamar user can log in and receive a usable local bearer token; an invalid login is rejected.

### Implementation for User Story 1

- [X] T011 [US1] Define the login request and response contracts in `backend/src/TajamarFaltas.Application/Auth/Models/`
- [X] T012 [US1] Implement the authentication service that validates the user against mirror data and external Tajamar identity rules in `backend/src/TajamarFaltas.Application/Auth/`
- [X] T013 [US1] Implement local JWT generation with role claims in `backend/src/TajamarFaltas.Application/Auth/TokenService.cs`
- [X] T014 [US1] Create the authentication controller and `POST /api/auth/login` endpoint in `backend/src/TajamarFaltas.Api/Controllers/AuthController.cs`
- [X] T015 [US1] Wire the login flow into `backend/src/TajamarFaltas.Api/Program.cs` with authentication and authorization middleware
- [X] T016 [US1] Ensure Scalar can authorize against the local JWT so protected endpoints can be exercised interactively from `backend/src/TajamarFaltas.Api/Program.cs`

**Checkpoint**: User Story 1 should be fully functional and testable independently

---

## Phase 4: User Story 2 - Consulta de faltas propias del alumno (Priority: P2)

**Goal**: Allow an authenticated alumno to read only their own absences.

**Independent Test**: An alumno sees only his or her own records and no data from other users.

### Implementation for User Story 2

- [X] T017 [US2] Define the alumno absence read models in `backend/src/TajamarFaltas.Application/FaltaManagement/Models/`
- [X] T018 [US2] Implement the alumno absence query service filtered by the authenticated user id in `backend/src/TajamarFaltas.Application/FaltaManagement/`
- [X] T019 [US2] Create the alumno controller endpoint for `GET /api/faltas/mis-faltas` in `backend/src/TajamarFaltas.Api/Controllers/FaltasController.cs`
- [X] T020 [US2] Add access checks so the endpoint only returns absences for the currently authenticated alumno in `backend/src/TajamarFaltas.Api/Authorization/`
- [X] T021 [US2] Add projection/mapping helpers from `Faltas` to API response models in `backend/src/TajamarFaltas.Application/Common/`

**Checkpoint**: User Story 2 should be fully functional and testable independently

---

## Phase 5: User Story 3 - Gestión de faltas del profesor (Priority: P3)

**Goal**: Allow a professor to view absences for an authorized course and create new absences within that scope.

**Independent Test**: An authenticated professor can query the assigned course and insert a valid absence for an authorized student.

### Implementation for User Story 3

- [X] T022 [US3] Define the profesor absence management request and response models in `backend/src/TajamarFaltas.Application/FaltaManagement/Models/`
- [X] T023 [US3] Implement the course-scoped professor query service in `backend/src/TajamarFaltas.Application/FaltaManagement/ProfesorFaltasService.cs`
- [X] T024 [US3] Implement the professor create-absence service with entity validation and foreign-key checks in `backend/src/TajamarFaltas.Application/FaltaManagement/ProfesorFaltasService.cs`
- [X] T025 [US3] Create the professor endpoints for `GET /api/profesor/cursos/{idCurso}/faltas` and `POST /api/profesor/faltas` in `backend/src/TajamarFaltas.Api/Controllers/ProfesorFaltasController.cs`
- [X] T026 [US3] Enforce authorization so professors can only operate on assigned courses in `backend/src/TajamarFaltas.Api/Authorization/`
- [X] T027 [US3] Add persistence logic for inserting new `Faltas` records in `backend/src/TajamarFaltas.Infrastructure/Persistence/`

**Checkpoint**: User Story 3 should be fully functional and testable independently

---

## Phase 6: User Story 4 - Supervisión total del administrador (Priority: P4)

**Goal**: Allow an administrator to list all absences and toggle their justification state.

**Independent Test**: An authenticated administrator can see the complete dataset and update the justification of an existing absence.

### Implementation for User Story 4

- [X] T028 [US4] Define the administrador absence update request and response models in `backend/src/TajamarFaltas.Application/FaltaManagement/Models/`
- [X] T029 [US4] Implement the administrator query service for all absences in `backend/src/TajamarFaltas.Application/FaltaManagement/AdminFaltasService.cs`
- [X] T030 [US4] Implement the justification update service for existing absences in `backend/src/TajamarFaltas.Application/FaltaManagement/AdminFaltasService.cs`
- [X] T031 [US4] Create the administrator endpoints for `GET /api/admin/faltas` and `PATCH /api/admin/faltas/{id}/justificacion` in `backend/src/TajamarFaltas.Api/Controllers/AdminFaltasController.cs`
- [X] T032 [US4] Enforce administrator-only authorization policies in `backend/src/TajamarFaltas.Api/Authorization/`
- [X] T033 [US4] Add persistence updates so `EsJustificada` changes are saved and reflected in subsequent reads in `backend/src/TajamarFaltas.Infrastructure/Persistence/`

**Checkpoint**: All user stories should now be independently functional

---

## Phase 7: Polish & Cross-Cutting Concerns

**Purpose**: Improvements that affect multiple user stories and documentation quality

- [ ] T034 [P] Add API documentation examples and usage notes for Scalar in `specs/001-faltas-api/quickstart.md`
- [ ] T035 [P] Refine shared error handling and validation responses in `backend/src/TajamarFaltas.Api/Middleware/` and `backend/src/TajamarFaltas.Application/Common/`
- [ ] T036 Verify the OpenAPI document and Scalar UI expose protected endpoints correctly from `backend/src/TajamarFaltas.Api/Program.cs`
- [ ] T037 Review and tighten authorization edge cases across `backend/src/TajamarFaltas.Api/Authorization/`
- [ ] T038 Confirm external Tajamar synchronization wiring remains isolated in `backend/src/TajamarFaltas.Infrastructure/ExternalServices/`
- [X] T034 [P] Add API documentation examples and usage notes for Scalar in `specs/001-faltas-api/quickstart.md`
- [X] T035 [P] Refine shared error handling and validation responses in `backend/src/TajamarFaltas.Api/Middleware/` and `backend/src/TajamarFaltas.Application/Common/`
- [X] T036 Verify the OpenAPI document and Scalar UI expose protected endpoints correctly from `backend/src/TajamarFaltas.Api/Program.cs`
- [X] T037 Review and tighten authorization edge cases across `backend/src/TajamarFaltas.Api/Authorization/`
- [X] T038 Confirm external Tajamar synchronization wiring remains isolated in `backend/src/TajamarFaltas.Infrastructure/ExternalServices/`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - blocks all user stories
- **User Stories (Phase 3+)**: All depend on Foundational phase completion
  - User stories can then proceed in priority order or in parallel once the foundation is in place
- **Polish (Final Phase)**: Depends on all desired user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational - no dependencies on other stories
- **User Story 2 (P2)**: Can start after Foundational - depends on the local auth model for identity claims
- **User Story 3 (P3)**: Can start after Foundational - depends on the local auth model and the shared `Faltas` persistence layer
- **User Story 4 (P4)**: Can start after Foundational - depends on the shared `Faltas` persistence layer

### Within Each User Story

- Shared models before controllers
- Service logic before endpoint wiring
- Authorization checks before exposing endpoints
- Core implementation before polish work
- Story complete before moving to the next priority

### Parallel Opportunities

- Setup tasks T001, T002, and T003 can be parallelized once the folder plan is agreed
- Foundational tasks T004, T005, T006, T007, T008, T009, and T010 touch different files and can mostly run in parallel
- User Stories 2, 3, and 4 can proceed in parallel after the foundation is ready, provided they do not conflict on shared files
- Scalar documentation work in T034 and T036 can be done independently of persistence tasks

---

## Parallel Example: User Story 3

```text
Task: "Define the profesor absence management request and response models in backend/src/TajamarFaltas.Application/FaltaManagement/Models/"
Task: "Enforce authorization so professors can only operate on assigned courses in backend/src/TajamarFaltas.Api/Authorization/"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational
3. Complete Phase 3: User Story 1
4. Stop and validate the login flow and Scalar token authorization
5. Demo the authenticated API shell before building the read/write features

### Incremental Delivery

1. Complete Setup + Foundational → foundation ready
2. Add User Story 1 → login and bearer token issuance
3. Add User Story 2 → alumno read-only access
4. Add User Story 3 → profesor scoped read/write access
5. Add User Story 4 → administrator oversight and justification updates
6. Finish with polish, documentation, and UI verification

### Parallel Team Strategy

With multiple developers:

1. Team completes Setup + Foundational together
2. Once the foundation is done:
   - Developer A: User Story 1
   - Developer B: User Story 2
   - Developer C: User Story 3
   - Developer D: User Story 4
3. Scalar and documentation updates can be handled alongside the feature work

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to a specific user story for traceability
- Each user story should be independently completable and testable
- This plan assumes the backend source tree does not yet exist and will be created under `backend/`
- Scalar is part of the API documentation surface, not a replacement for authentication or authorization