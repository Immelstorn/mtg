$("#plusBtn").on('click', function () {
    changeLife(true);
});

$("#minusBtn").on('click', function () {
    changeLife(false);
});

$('#viewGraveBtn').on('click', function () {
    viewCards(ViewFrom.Graveyard);
});

$('#viewExile').on('click', function () {
    viewCards(ViewFrom.Exile);
});

$('.card').on("DOMAttrModified", onDOMAttrModified);

$('#changeOrderBtn').on('click', function() {
    changeOrderOfCards();
});