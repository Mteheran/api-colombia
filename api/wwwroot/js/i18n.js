let translations = {};
const languageSelector = document.getElementById('languageSelector');
const languageSelectorDisplay = document.querySelector('.language-selector-display');
const languageDropdown = document.getElementById('languageDropdown');
const selectedFlag = document.getElementById('selectedFlag');
const languageText = document.getElementById('languageText');

// Selector móvil
const languageSelectorMobile = document.getElementById('languageSelectorMobile');
const languageSelectorDisplayMobile = document.querySelector('.language-selector-display-mobile');
const languageDropdownMobile = document.getElementById('languageDropdownMobile');
const selectedFlagMobile = document.getElementById('selectedFlagMobile');
const languageTextMobile = document.getElementById('languageTextMobile');

async function fetchTranslations() {
  try {
    const response = await fetch('/js/translations.json');
    if (!response.ok) {
      throw new Error('Failed to load translations.');
    }
    translations = await response.json();
    setInitialLanguage();
  } catch (error) {
    // Error loading translations
  }
}

function getBrowserLanguage() {
  const browserLang = navigator.language || navigator.userLanguage;
  const langCode = browserLang.toLowerCase().split('-')[0];
  
  const supportedLanguages = ['es', 'en', 'pt'];
  
  if (supportedLanguages.includes(langCode)) {
    return langCode;
  }
  
  return 'es';
}

function setInitialLanguage() {
  const storedLang = localStorage.getItem('language');
  const defaultLang = storedLang || getBrowserLanguage();
  
  document.documentElement.setAttribute('lang', defaultLang);
  translatePage(defaultLang);
  updateFlagAndText(defaultLang);
}

function translatePage(lang) {
  document.querySelectorAll('[data-i18n-key]').forEach(element => {
    const key = element.getAttribute('data-i18n-key');
    if (translations[lang] && translations[lang][key]) {
      element.textContent = translations[lang][key];
    }
  });
}

function getTranslation(key) {
  const currentLang = document.documentElement.getAttribute('lang') || 'es';
  return translations[currentLang] && translations[currentLang][key] 
    ? translations[currentLang][key] 
    : key;
}

// Hacer la función disponible globalmente
window.getTranslation = getTranslation;

function updateFlagAndText(lang) {
  const flagMap = {
    'es': '/assets/es.png',
    'en': '/assets/en.png',
    'pt': '/assets/pr.png'
  };
  
  const languageNames = {
    'es': 'Español',
    'en': 'English',
    'pt': 'Português'
  };
  
  const newFlagSrc = flagMap[lang];
  const languageName = languageNames[lang];
  
  // Actualizar selector desktop
  if (selectedFlag && languageText) {
    selectedFlag.src = newFlagSrc;
    languageText.textContent = lang.toUpperCase();
  }
  
  if (languageSelectorDisplay) {
    languageSelectorDisplay.setAttribute('aria-label', `Idioma actual: ${languageName}. Haz clic para cambiar`);
  }
  
  // Actualizar selector móvil
  if (selectedFlagMobile && languageTextMobile) {
    selectedFlagMobile.src = newFlagSrc;
    languageTextMobile.textContent = lang.toUpperCase();
  }
  
  if (languageSelectorDisplayMobile) {
    languageSelectorDisplayMobile.setAttribute('aria-label', `Idioma actual: ${languageName}. Haz clic para cambiar`);
  }
}

function detectBrowser() {
  const userAgent = navigator.userAgent;
  
  if (userAgent.includes('Firefox')) {
    return 'Firefox';
  } else if (userAgent.includes('Edg')) {
    return 'Edge';
  } else if (userAgent.includes('Chrome')) {
    return 'Chrome';
  } else if (userAgent.includes('Safari') && !userAgent.includes('Chrome')) {
    return 'Safari';
  } else if (userAgent.includes('Opera') || userAgent.includes('OPR')) {
    return 'Opera';
  } else {
    return 'Unknown';
  }
}

function detectOS() {
  const userAgent = navigator.userAgent;
  const platform = navigator.platform;
  
  if (userAgent.includes('Win') || platform.includes('Win')) return 'Windows';
  if (userAgent.includes('Mac') || platform.includes('Mac')) return 'MacOS';
  if (userAgent.includes('Linux') || platform.includes('Linux')) return 'Linux';
  if (userAgent.includes('Android')) return 'Android';
  if (userAgent.includes('iOS') || userAgent.includes('iPhone') || userAgent.includes('iPad')) return 'iOS';
  
  return 'Unknown';
}

function isMobileDevice() {
  return /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
}

function getBrowserInfo() {
  const info = {
    browser: detectBrowser(),
    os: detectOS(),
    isMobile: isMobileDevice(),
    language: navigator.language || navigator.userLanguage,
    languages: navigator.languages || [navigator.language],
    userAgent: navigator.userAgent,
    platform: navigator.platform,
    online: navigator.onLine
  };
    return info;
}

// Función para cambiar idioma (usada por ambos selectores)
function changeLanguage(newLang) {
  document.documentElement.setAttribute('lang', newLang);
  localStorage.setItem('language', newLang);
  
  translatePage(newLang);
  updateFlagAndText(newLang);
  
  // Cerrar dropdowns
  if (languageDropdown) {
    languageDropdown.classList.remove('active');
  }
  if (languageDropdownMobile) {
    languageDropdownMobile.classList.remove('active');
  }
  
  // Disparar evento personalizado para notificar a otros módulos
  const languageChangeEvent = new CustomEvent('languageChanged', { detail: { language: newLang } });
  window.dispatchEvent(languageChangeEvent);
}

// Toggle dropdown desktop
if (languageSelectorDisplay) {
  languageSelectorDisplay.addEventListener('click', (e) => {
    e.stopPropagation();
    const isExpanded = languageDropdown.classList.toggle('active');
    languageSelectorDisplay.setAttribute('aria-expanded', isExpanded);
    // Cerrar el móvil si está abierto
    if (languageDropdownMobile) {
      languageDropdownMobile.classList.remove('active');
      if (languageSelectorDisplayMobile) {
        languageSelectorDisplayMobile.setAttribute('aria-expanded', 'false');
      }
    }
    // Focus en primera opción si se abre
    if (isExpanded) {
      const firstOption = languageDropdown.querySelector('.language-option');
      if (firstOption) firstOption.focus();
    }
  });
  
  // Soporte de teclado
  languageSelectorDisplay.addEventListener('keydown', (e) => {
    if (e.key === 'Enter' || e.key === ' ') {
      e.preventDefault();
      languageSelectorDisplay.click();
    }
  });
}

// Toggle dropdown móvil
if (languageSelectorDisplayMobile) {
  languageSelectorDisplayMobile.addEventListener('click', (e) => {
    e.stopPropagation();
    const isExpanded = languageDropdownMobile.classList.toggle('active');
    languageSelectorDisplayMobile.setAttribute('aria-expanded', isExpanded);
    // Cerrar el desktop si está abierto
    if (languageDropdown) {
      languageDropdown.classList.remove('active');
      if (languageSelectorDisplay) {
        languageSelectorDisplay.setAttribute('aria-expanded', 'false');
      }
    }
    // Focus en primera opción si se abre
    if (isExpanded) {
      const firstOption = languageDropdownMobile.querySelector('.language-option-mobile');
      if (firstOption) firstOption.focus();
    }
  });
  
  // Soporte de teclado
  languageSelectorDisplayMobile.addEventListener('keydown', (e) => {
    if (e.key === 'Enter' || e.key === ' ') {
      e.preventDefault();
      languageSelectorDisplayMobile.click();
    }
  });
}

// Event listeners para opciones del dropdown desktop
if (languageDropdown) {
  const options = languageDropdown.querySelectorAll('.language-option');
  options.forEach((option, index) => {
    option.addEventListener('click', () => {
      const lang = option.getAttribute('data-lang');
      changeLanguage(lang);
    });
    
    // Navegación por teclado
    option.addEventListener('keydown', (e) => {
      if (e.key === 'Enter' || e.key === ' ') {
        e.preventDefault();
        const lang = option.getAttribute('data-lang');
        changeLanguage(lang);
      } else if (e.key === 'ArrowDown') {
        e.preventDefault();
        const nextOption = options[index + 1];
        if (nextOption) nextOption.focus();
      } else if (e.key === 'ArrowUp') {
        e.preventDefault();
        const prevOption = options[index - 1];
        if (prevOption) prevOption.focus();
        else if (languageSelectorDisplay) languageSelectorDisplay.focus();
      } else if (e.key === 'Escape') {
        e.preventDefault();
        languageDropdown.classList.remove('active');
        languageSelectorDisplay.setAttribute('aria-expanded', 'false');
        languageSelectorDisplay.focus();
      }
    });
  });
}

// Event listeners para opciones del dropdown móvil
if (languageDropdownMobile) {
  const optionsMobile = languageDropdownMobile.querySelectorAll('.language-option-mobile');
  optionsMobile.forEach((option, index) => {
    option.addEventListener('click', () => {
      const lang = option.getAttribute('data-lang');
      changeLanguage(lang);
    });
    
    // Navegación por teclado
    option.addEventListener('keydown', (e) => {
      if (e.key === 'Enter' || e.key === ' ') {
        e.preventDefault();
        const lang = option.getAttribute('data-lang');
        changeLanguage(lang);
      } else if (e.key === 'ArrowDown') {
        e.preventDefault();
        const nextOption = optionsMobile[index + 1];
        if (nextOption) nextOption.focus();
      } else if (e.key === 'ArrowUp') {
        e.preventDefault();
        const prevOption = optionsMobile[index - 1];
        if (prevOption) prevOption.focus();
        else if (languageSelectorDisplayMobile) languageSelectorDisplayMobile.focus();
      } else if (e.key === 'Escape') {
        e.preventDefault();
        languageDropdownMobile.classList.remove('active');
        languageSelectorDisplayMobile.setAttribute('aria-expanded', 'false');
        languageSelectorDisplayMobile.focus();
      }
    });
  });
}

// Cerrar dropdown cuando se hace clic fuera
document.addEventListener('click', (e) => {
  if (languageDropdown && !languageSelector.contains(e.target)) {
    languageDropdown.classList.remove('active');
    if (languageSelectorDisplay) {
      languageSelectorDisplay.setAttribute('aria-expanded', 'false');
    }
  }
  if (languageDropdownMobile && !languageSelectorMobile.contains(e.target)) {
    languageDropdownMobile.classList.remove('active');
    if (languageSelectorDisplayMobile) {
      languageSelectorDisplayMobile.setAttribute('aria-expanded', 'false');
    }
  }
});

// Cerrar dropdowns con Escape
document.addEventListener('keydown', (e) => {
  if (e.key === 'Escape') {
    if (languageDropdown && languageDropdown.classList.contains('active')) {
      languageDropdown.classList.remove('active');
      if (languageSelectorDisplay) {
        languageSelectorDisplay.setAttribute('aria-expanded', 'false');
        languageSelectorDisplay.focus();
      }
    }
    if (languageDropdownMobile && languageDropdownMobile.classList.contains('active')) {
      languageDropdownMobile.classList.remove('active');
      if (languageSelectorDisplayMobile) {
        languageSelectorDisplayMobile.setAttribute('aria-expanded', 'false');
        languageSelectorDisplayMobile.focus();
      }
    }
  }
});

document.addEventListener('DOMContentLoaded', () => {
  getBrowserInfo();
  fetchTranslations();
});