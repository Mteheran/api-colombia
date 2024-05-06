const apiBaseUrl = "https://api-colombia.com/api/";
const cache = new Map(); // Mapeo para almacenar las respuestas en caché

// Obtengo los elementos necesarios por su ID o clase
const selectVersion = document.getElementById("selectVersion");
const selectData = document.getElementById("selectData");
const btnSolicitar = document.getElementById("btnSolicitar");
const resultadoDiv = document.querySelector(".resultado");

async function fetchData(apiUrl) {
  try {
    // Verifico si la respuesta está en caché
    if (cache.has(apiUrl)) {
      console.log("Datos obtenidos de la caché");
      return cache.get(apiUrl);
    }

    // Realizo la solicitud FETCH a la API
    const response = await fetch(apiUrl);
    if (!response.ok) {
      throw new Error("Error en la solicitud: " + response.statusText);
    }

    // Convierto la respuesta a JSON
    const data = await response.json();

    // Almaceno la respuesta en caché
    cache.set(apiUrl, data);

    return data;
  } catch (error) {
    throw new Error("Error al obtener los datos: " + error.message);
  }
}

// Agrego un evento al botón de solicitar datos
btnSolicitar.addEventListener("click", async function () {
  const version = selectVersion.value;
  const data = selectData.value;
  let apiUrl = "";

  if (data === "CountryColombia") {
    apiUrl = "https://api-colombia.com/api/v1/country/Colombia";
  } else {
    apiUrl = `https://api-colombia.com/api/${version}/${data}`;
  }

  try {
    const responseData = await fetchData(apiUrl);
    console.log(responseData);
    resultadoDiv.innerText = JSON.stringify(responseData, null, 2);
  } catch (error) {
    console.error("Error al obtener los datos:", error);
    resultadoDiv.innerText = "Error al obtener los datos";
  }
});

// Función para simular la solicitud a la API
async function fetchData(url) {
  // Simulando una solicitud fetch a la API
  const response = await fetch(url);
  const data = await response.json();
  return data;
}

// ESTE ES EL CONSUMO DE COUNTRY COLOMBIA EN LA CUAL DE MANERA ASINCRONA SE CONSUME
btnSolicitar.addEventListener("click", async function () {
  // Agrego un evento al botón
  // Obtengo el valor de la versión y los datos seleccionados
  const version = selectVersion.value;
  const data = selectData.value;

  // Construyo la URL de la API basada en la versión y los datos seleccionados
  let apiUrl = "";

  // Verificar si se seleccionó el nuevo endpoint
  if (data === "CountryColombia") {
    apiUrl = "https://api-colombia.com/api/v1/country/Colombia";
  } else {
    apiUrl = `${apiBaseUrl}${version}/${data}`;
  }

  try {
    // Obtengo los datos de la API
    const responseData = await fetchData(apiUrl);

    // Proceso los datos obtenidos
    console.log(responseData);

    // Muestro los datos en el div resultado
    resultadoDiv.innerText = JSON.stringify(responseData, null, 2);
  } catch (error) {
    // Aqui capturo y manejo los errores de la solicitud a la API
    console.error("Error al obtener los datos:", error);

    // Muestro un mensaje de error en el div resultado cuando no se consuma correctamente
    resultadoDiv.innerText = "Error al obtener los datos";
  }
});

// ESTA FUNCIÓN ES PARA HACERLE SAER AL USUARIO QUE ENDPOINT SE ESTA CONSUMIENDO EN LA API
document.getElementById("selectData").addEventListener("change", function () {
  var selectedOption = this.value;
  document.getElementById("endpoint").textContent = selectedOption + "'";
});

// Ejecución del api luego de cargada la pagina htm completa
document.addEventListener("DOMContentLoaded", async function () {
  const apiUrl = "https://api-colombia.com/api/v1/department";

  try {
    const responseData = await fetchData(apiUrl);
    console.log(responseData);
    resultadoDiv.innerText = JSON.stringify(responseData, null, 2);
  } catch (error) {
    console.error("Error al obtener los datos:", error);
    resultadoDiv.innerHTML =
      "<p>No se encontraron datos. Vuelva a intentarlo</p>";
  }
});
