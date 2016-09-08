var WhereToMove = {
    NA: 0,
    Top: 1,
    Bottom: 2,
    Graveyard: 3,
    Exile: 4,
    Hand: 5,
    Battlefield:6
};

var MoveFrom = {
    NA: 0,
    Top: 1,
    Hand: 2,
    Graveyard: 3,
    Exile: 4,
    Battlefield: 5,
};

var ViewFrom =
{
    Graveyard: 1,
    Exile: 2,
    EnemyHand: 3,
    TopOfTheDeck: 4,
}

var Counters =
{
    Plus1: 1,
    Minus1: 2,
    Fade: 3,
    Time: 4,
    Level: 5,
    Loyality: 6
}

var activeAjaxRequests = 0;

//document.ready
$(function () {
    $.ajaxSetup({
        data: {
            gameID: $('#gameID').val()
        }
    });
  
    $(document).ajaxSend(onAjaxSend);
    $(document).ajaxComplete(onAjaxComplete);

    loadGame();
});