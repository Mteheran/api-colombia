const botonMenu = document.getElementById("hamburguer");
const modal = document.querySelector(".modal");
const closeButton = document.getElementById("closeModal");
const botonColor = document.getElementById("modoWeb");
const bodyContent = document.body;
const modale= document.getElementsByClassName("contenidoModal");
const imagencambia = document.getElementById("Logo-API");
const selectTheme = document.getElementById("select-theme");
const mododod = document.getElementById("modoWeb2")
const botonDocumentacion = document.getElementsByClassName("botonDocumentacion");

Array.from(botonDocumentacion).forEach(element => {
    element.addEventListener("click", () => {
        window.open("https://api-colombia.com/swagger/index.html",'_blank');
        
    
})});


toggleModal(botonMenu,modal);
toggleModal(closeButton,modal);

toggleColor(botonColor,bodyContent,imagencambia);
toggleColor2(selectTheme,modale[0],mododod,bodyContent,imagencambia);



