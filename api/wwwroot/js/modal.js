const toggleModal = (button, modal) => {
    button.addEventListener("click", () => {
        modal.classList.toggle("show");
        modal.classList.toggle("hidden");
    });
};

const toggleColor = (button,body,imagen) =>{
    let dark = true;
    button.addEventListener("click", () =>{
        dark = !dark; 

        body.classList.toggle("dark");

        if (!dark) {
            button.src = "assets/icons/moon-icon.svg"; 
            imagen.src = "assets/logo-dark.svg"
            
        } else {
            button.src = "assets/icons/sun-icon.svg";
            imagen.src = "assets/logo-light.svg"
        }
        
    });
};

const toggleColor2 = (button,body,imagen,cuerpo,imagen2) =>{
    let dark = true;
    button.addEventListener("change", () =>{
        dark = !dark; 
        cuerpo.classList.toggle("dark");
        body.classList.toggle("dark");

        if (!dark) { 
            imagen.src = "assets/icons/moon-icon.svg"
            imagen2.src= "assets/logo-dark.svg"
            
        } else {
            imagen.src = "assets/icons/sun-icon.svg"
            imagen2.src = "assets/logo-light.svg"
        }
        
    });
};



