const selector = document.getElementById('languageSelector');
const flag = document.getElementById('selectedFlag');

function translatePage(lang) {
  const elements = document.querySelectorAll('[data-translate]');
  elements.forEach((el) => {
    const key = el.getAttribute('data-translate');
    const translation = window.translations[lang][key];
    if (translation) {
      el.innerHTML = translation;
    }
  });

  const p = document.querySelectorAll('.sponsorships-responsive p');
  if (p.length >= 3) {
    p[0].innerText = window.translations[lang].sponsorText1;
    p[1].innerText = window.translations[lang].sponsorText2;
    p[2].innerText = window.translations[lang].sponsorText3;
  }

  const sponsorBtn = document.querySelector('.sponsor p');
  if (sponsorBtn) sponsorBtn.innerText = window.translations[lang].sponsor;

  const madeWith = document.querySelector('.made span');
  if (madeWith) madeWith.innerText = window.translations[lang].madeWith;
}

selector.addEventListener('change', (e) => {
  const selected = e.target.options[e.target.selectedIndex];
  const flagUrl = selected.dataset.flag;
  const lang = selected.value;

  flag.src = flagUrl;
  localStorage.setItem('lang', lang);
  translatePage(lang);
});

document.addEventListener('DOMContentLoaded', () => {
  const lang = localStorage.getItem('lang') || 'es';
  selector.value = lang;
  const option = selector.querySelector(`option[value="${lang}"]`);
  if (option) {
    flag.src = option.dataset.flag;
  }
  translatePage(lang);
});
