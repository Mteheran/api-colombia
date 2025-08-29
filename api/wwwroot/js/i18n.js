let translations = {};
const languageSelector = document.getElementById('languageSelector');

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

function setInitialLanguage() {
  const storedLang = localStorage.getItem('language') || 'es';
  languageSelector.value = storedLang;
  translatePage(storedLang);
}

function translatePage(lang) {
  document.querySelectorAll('[data-i18n-key]').forEach(element => {
    const key = element.getAttribute('data-i18n-key');
    if (translations[lang] && translations[lang][key]) {
      element.innerText = translations[lang][key];
    }
  });
}

languageSelector.addEventListener('change', (event) => {
  const newLang = event.target.value;
  localStorage.setItem('language', newLang);
  translatePage(newLang);
});

document.addEventListener('DOMContentLoaded', fetchTranslations);
