/* const selector = document.getElementById('languageSelector');
const elements = document.querySelectorAll('[data-i18n]');

const translations = {
  es: { title: "Explora Colombia con API-Colombia", subtitle: "Un proyecto Open-source...", documentation: "Documentación" },
  en: { title: "Explore Colombia with API-Colombia", subtitle: "An open-source project...", documentation: "Documentation" },
  pt: { title: "Explore a Colômbia com API-Colombia", subtitle: "Um projeto open-source...", documentation: "Documentação" }
};

function setLanguage(lang) {
  localStorage.setItem('lang', lang);
  elements.forEach(el => {
    const key = el.getAttribute('data-i18n');
    if (translations[lang] && translations[lang][key]) {
      el.textContent = translations[lang][key];
    }
  });
}

selector.addEventListener('change', e => {
  setLanguage(e.target.value);
});

document.addEventListener('DOMContentLoaded', () => {
  const savedLang = localStorage.getItem('lang') || 'es';
  selector.value = savedLang;
  setLanguage(savedLang);
});
 */

  const selector = document.getElementById('languageSelector');
  const flag = document.getElementById('selectedFlag');

  selector.addEventListener('change', (e) => {
    const selected = e.target.options[e.target.selectedIndex];
    const flagUrl = selected.dataset.flag; 
    flag.src = flagUrl;
    localStorage.setItem('lang', selected.value);
  });

  document.addEventListener('DOMContentLoaded', () => {
    const lang = localStorage.getItem('lang') || 'es';
    selector.value = lang;
    const option = selector.querySelector(`option[value="${lang}"]`);
    if (option) {
      flag.src = option.dataset.flag;
    }
  });