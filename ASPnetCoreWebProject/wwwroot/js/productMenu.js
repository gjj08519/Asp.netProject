document.addEventListener("DOMContentLoaded", () => {
    // Change the displayed price when the chosen portion is changed
    // Assisted by https://stackoverflow.com/questions/51573435/want-to-add-addeventlistener-on-multiple-elements-with-same-class
    [...document.querySelectorAll(".select-product-portion")].forEach(portionSelect => {
        portionSelect.addEventListener("change", (event) => {
            let productId = event.srcElement.getAttribute("product-id");
            let selectedPortionOption = event.srcElement.options[event.srcElement.selectedIndex];
            let portionIndex = selectedPortionOption.getAttribute("portion-index");

            // Hide the outdatedPriceElement, and show the newPriceElement
            let outdatedPriceElement = document.querySelector(`.product-price[product-id='${productId}'][style*='display: block']`);
            let newPriceElement = document.querySelector(`.product-price[product-id='${productId}'][portion-index='${portionIndex}']`);

            outdatedPriceElement.style.display = 'none';
            newPriceElement.style.display = 'block';
        });
    });

    // Add an item to the cart when an "Add to Cart" button is clicked
    [...document.querySelectorAll(".product-add-to-cart")].forEach(addToCartBtn => {
        addToCartBtn.addEventListener("click", (event) => {
            let productId = event.srcElement.getAttribute("product-id");

            // get the selected portion
            let portionName = document.querySelector(`.select-product-portion[product-id='${productId}']`).value;

            // get the selected quantity
            let quantity = document.querySelector(`.select-product-quantity[product-id='${productId}']`).value;

            // update "Add to cart" form with the portion and quantity
            let chosenProductPortion = document.querySelector(`.chosen-portion[product-id='${productId}']`);
            let chosenProductQuantity = document.querySelector(`.chosen-quantity[product-id='${productId}']`);
            chosenProductPortion.value = portionName;
            chosenProductQuantity.value = quantity;
        });
    });
});
