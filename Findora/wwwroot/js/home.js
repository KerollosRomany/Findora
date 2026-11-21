(function () {
  'use strict';

  var governorate = document.getElementById('homeGovernorate');
  var city = document.getElementById('homeCity');
  if (!governorate || !city) return;

  function setOnly(text) {
    city.innerHTML = '';
    var option = document.createElement('option');
    option.value = '';
    option.textContent = text;
    city.appendChild(option);
  }

  governorate.addEventListener('change', async function () {
    var governorateId = governorate.value;

    if (!governorateId) {
      setOnly('Select a governorate first');
      city.disabled = true;
      return;
    }

    setOnly('Loading cities...');
    city.disabled = true;

    try {
      var response = await fetch('/Account/GetCities?governorateId=' + encodeURIComponent(governorateId));
      if (!response.ok) throw new Error('Failed to load cities');

      var cities = await response.json();
      setOnly(cities.length ? 'Any city' : 'No cities yet');

      cities.forEach(function (c) {
        var option = document.createElement('option');
        option.value = c.id;
        option.textContent = c.name;
        city.appendChild(option);
      });

      city.disabled = cities.length === 0;
    } catch (error) {
      console.error(error);
      setOnly('Could not load cities');
    }
  });
})();
