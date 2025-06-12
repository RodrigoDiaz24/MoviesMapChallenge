# CineMap - Backend .NET API

Este proyecto es el backend de CineMap, donde se encuentran endpoints que devuelven locaciones de películas filmadas en San Francisco, utilizando datos obtenidos desde la API pública de DataSF.

---

## Herramientas y dependencias principales

- **ASP.NET Core 9.0.4**
- **HttpClient** (para consumir API externa)
- **xUnit** (para testing)
- **Moq** (para mocking en tests)
- **CORS** habilitado para Angular (localhost:4200)

3. La API estará disponible en:
https://localhost:7013


## Endpoints disponibles

### Obtener películas cercanas a una ubicación endpoint:

```
GET /api/movies/nearby
```

### Obtener todas las películas disponibles endpoint:

```
GET /api/movies
```

## Recomendación para levantar la solución

Abrí la solución `MoviesChallenge.sln` y seleccioná como proyecto de inicio:
MoviesChallenge.API

Y ejecutar la aplicacion. 
