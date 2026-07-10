const botonMenu = document.getElementById("hamburguer");
const modal = document.querySelector(".modal");
const closeButton = document.getElementById("closeModal");
const botonColor = document.getElementById("modoWeb");
const bodyContent = document.body;
const modale= document.getElementsByClassName("contenidoModal");
const imagencambia = document.getElementById("Logo-API");
const selectTheme = document.getElementById("select-theme");
const mododod = document.getElementById("modoWeb2")
const btnSwagger = document.getElementById("btnSwagger");
const btnVitePress = document.getElementById("btnVitePress");
const btnMcp = document.getElementById("btnMcp");

if (btnSwagger) {
    btnSwagger.addEventListener("click", () => {
        window.open("https://api-colombia.com/swagger/index.html", '_blank');
    });
}

if (btnVitePress) {
    btnVitePress.addEventListener("click", () => {
        window.open("https://docs.api-colombia.com/", '_blank');
    });
}

if (btnMcp) {
    // Relative URL so it targets the current origin (localhost during development,
    // api-colombia.com in production). Serves wwwroot/mcp-inspector.html via the /mcp route.
    btnMcp.addEventListener("click", () => {
        window.open("/mcp", '_blank');
    });
}


toggleModal(botonMenu,modal);
toggleModal(closeButton,modal);

toggleColor(botonColor,bodyContent,imagencambia);
toggleColor2(selectTheme,modale[0],mododod,bodyContent,imagencambia);



