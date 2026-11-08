
const governorateDropdown = document.getElementById("governorateDropdown");
const cityDropdown = document.getElementById("cityDropdown");

governorateDropdown.addEventListener("change", async function () {

    const governorateId = this.value;

    cityDropdown.innerHTML =
        '<option value="">Select City</option>';

    cityDropdown.disabled = true;

    if (!governorateId)
        return;

    try {
        const response = await fetch(
            `/Account/GetCities?governorateId=${governorateId}`
        );

        if (!response.ok)
            throw new Error("Failed to load cities");

        const cities = await response.json();

        cities.forEach(city => {
            const option = document.createElement("option");

            option.value = city.id;
            option.textContent = city.name;

            cityDropdown.appendChild(option);
        });

        cityDropdown.disabled = false;
    }
    catch (error) {
        console.error(error);
    }
});
