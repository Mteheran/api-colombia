const apiBaseUrl = "https://api-colombia.com/api/";
const cache = new Map(); // Mapeo para almacenar las respuestas en caché

// Obtengo los elementos necesarios por su ID o clase
const selectVersion = document.getElementById("selectVersion");
const selectData = document.getElementById("selectData");
const btnSolicitar = document.getElementById("btnSolicitar");
const selectYear = document.getElementById("selectYear");
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

const currentYear = new Date().getFullYear();
for (let year = currentYear; year >= 2000; year--) {
  let option = document.createElement("option");
  option.value = year;
  option.textContent = year;
  selectYear.appendChild(option);
}

selectData.addEventListener("change", function () {
  if (this.value === "Holiday") {
    selectYear.style.display = "inline-block";
  } else {
    selectYear.style.display = "none";
  }
});


async function fetchData(apiUrl) {
  try {
    if (cache.has(apiUrl)) {
      console.log("✅ Datos obtenidos de la caché:", apiUrl);
      return cache.get(apiUrl);
    }

    const response = await fetch(apiUrl, {
      headers: {
        "Accept": "application/json"
      }
    });

    if (!response.ok) {
      await new Promise(resolve => setTimeout(resolve, 1000));
      throw new Error(`Error en la solicitud: ${response.status} - ${response.statusText}`);
    }

    const data = await response.json();
    cache.set(apiUrl, data);
    return data;
  } catch (error) {
    await new Promise(resolve => setTimeout(resolve, 1000));
    throw new Error("Error al obtener los datos: " + error.message);
  }
}


// Agrego un evento al botón de solicitar datos
/* btnSolicitar.addEventListener("click", async function () {
  const version = selectVersion.value;
  const data = selectData.value;
  let apiUrl = "";

  if (data === "CountryColombia") {
    apiUrl = "https://api-colombia.com/api/v1/country/Colombia";
  } else if (data === "Holiday") {
    const year = selectYear.value;
    apiUrl = `https://api-colombia.com/api/${version}/Holiday/year/${year}`;
  } else {
    apiUrl = `${apiBaseUrl}${version}/${data}`;
  }

  try {
    const responseData = await fetchData(apiUrl);
    resultadoDiv.innerText = JSON.stringify(responseData, null, 2);

  } catch (error) {
    console.error("Error al obtener los datos:", error);
  }
}); */

// Muestra en la UI qué endpoint se está consumiendo
document.getElementById("selectData").addEventListener("change", function () {
  var selectedOption = this.value;
  document.getElementById("endpoint").textContent = selectedOption + "'";
});

// Cargar datos iniciales al cargar la página
document.addEventListener("DOMContentLoaded", async function () {
  const apiUrl = "https://api-colombia.com/api/v1/department";

  try {
    const responseData = await fetchData(apiUrl);
    console.log(responseData);
    resultadoDiv.innerText = JSON.stringify(responseData, null, 2);
  } catch (error) {
    resultadoDiv.innerHTML = "<p>No se encontraron datos. Vuelva a intentarlo</p>";
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
    } else if (data === "Holiday") {
        const year = selectYear.value;
        apiUrl = `https://api-colombia.com/api/${version}/Holiday/year/${year}`;
    }
    else {
        apiUrl = `${apiBaseUrl}${version}/${data}`;
    }

    try {
        resultadoDiv.innerText = "Cargando datos...";
        await new Promise(resolve => setTimeout(resolve, 1000));
        const responseData = await fetchData(apiUrl);

        let truncatedData = responseData;
        if (Array.isArray(responseData)) {
            truncatedData = responseData.slice(0, 200); // Limitar a solo 200 resultados para evitar que se bloquee la UI
        } else if (typeof responseData === "object") {
            truncatedData = Object.keys(responseData)
                .slice(0, 200)
                .reduce((obj, key) => {
                    obj[key] = responseData[key];
                    return obj;
                }, {});
        }

        resultadoDiv.innerText = JSON.stringify(truncatedData, null, 2);
    } catch (error) {
        // Aqui capturo y manejo los errores de la solicitud a la API

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
