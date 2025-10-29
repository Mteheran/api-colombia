let translations = {};
const languageSelector = document.getElementById('languageSelector');
const selectedFlag = document.getElementById('selectedFlag');
const languageText = document.getElementById('languageText');

// Selector móvil
const languageSelectorMobile = document.getElementById('languageSelectorMobile');
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
    console.error(error);
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
  
  languageSelector.value = defaultLang;
  if (languageSelectorMobile) {
    languageSelectorMobile.value = defaultLang;
  }
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
  const selectedOption = languageSelector.options[languageSelector.selectedIndex];
  const newFlagSrc = selectedOption.getAttribute('data-flag');
  
  // Actualizar selector desktop
  selectedFlag.src = newFlagSrc;
  languageText.textContent = lang.toUpperCase();
  
  // Actualizar selector móvil
  if (languageSelectorMobile && selectedFlagMobile && languageTextMobile) {
    selectedFlagMobile.src = newFlagSrc;
    languageTextMobile.textContent = lang.toUpperCase();
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
  
  // Sincronizar ambos selectores
  languageSelector.value = newLang;
  if (languageSelectorMobile) {
    languageSelectorMobile.value = newLang;
  }
  
  translatePage(newLang);
  updateFlagAndText(newLang);
  
  // Disparar evento personalizado para notificar a otros módulos
  const languageChangeEvent = new CustomEvent('languageChanged', { detail: { language: newLang } });
  window.dispatchEvent(languageChangeEvent);
}

// Listener para selector desktop
languageSelector.addEventListener('change', (event) => {
  changeLanguage(event.target.value);
});

// Listener para selector móvil
if (languageSelectorMobile) {
  languageSelectorMobile.addEventListener('change', (event) => {
    changeLanguage(event.target.value);
  });
}

document.addEventListener('DOMContentLoaded', () => {
  getBrowserInfo();
  fetchTranslations();
});