$.contextMenu({
    selector: '#deck',
    items: {
        "draw": {
            name: "Draw a card",
            callback: function () {
                drawACard();
            }
        },
        "lookAtTheTop": {
            name: "Look at the top X cards",
            type: 'text',
            value: "1",
            events: {
                keyup: function (e) {
                    if (e.keyCode == 13) {
                        lookAtTheTop($(this).val());
                    }
                }
            }
        }
    }
});

$.contextMenu({
    selector: '.card.inGrave, .card.inExile',
    items: {
        "info": {
            name: "Info",
            callback: function () {
                prepareInfoModal(this);
            }
        }
    }
});

$.contextMenu({
    selector: '#myBattlefield',
    items: {
        "addToken": {
            name: "Add token",
            callback: function () {
                addToken();
            }
        }
    }
});

$.contextMenu({
    selector: '.counter',
    items: {
        "remove": {
            name: "Remove",
            callback: function () {
                removeCounter(this);
            }
        }
    }
});

$.contextMenu({
    selector: '#viewCardsModal .card.onTopOfTheDeck',
    items: {
        "info": {
            name: "Info",
            callback: function () {
                prepareInfoModal(this);
            }
        },
        "move": {
            name: "Move to",
            items: {
                "bottom": {
                    name: "Bottom of the deck",
                    callback: function () {
                        moveTo(this, WhereToMove.Bottom, MoveFrom.Top);
                    }
                },
                "grave": {
                    name: "Graveyard",
                    callback: function () {
                        moveTo(this, WhereToMove.Graveyard, MoveFrom.Top);
                    }
                },
                "exile": {
                    name: "Exile",
                    callback: function () {
                        moveTo(this, WhereToMove.Exile, MoveFrom.Top);
                    }
                },
                "hand": {
                    name: "Hand",
                    callback: function () {
                        moveTo(this, WhereToMove.Hand, MoveFrom.Top);
                    }
                }
            }
        },
    }
});

$.contextMenu({
    selector: '.card.inGame',
    items: {
        "tapUntap": {
            name: "Tap/Untap",
            callback: function() {
                tapUntap(this);
            }
        },
        "info": {
            name: "Info",
            callback: function() {
                prepareInfoModal(this);
            }
        },
        "counters":
        {
            name: "Add counter",
            items: {
                "plus1":
                {
                    name: "+1/+1 counter",
                    callback: function() {
                        createCounter(this, Counters.Plus1);
                    }
                },
                "minus1":
                {
                    name: "-1/-1 counter",
                    callback: function() {
                        createCounter(this, Counters.Minus1);
                    }
                },
                "fade":
                {
                    name: "Fade counter",
                    callback: function() {
                        createCounter(this, Counters.Fade);
                    }
                },
                "time":
                {
                    name: "Time counter",
                    callback: function() {
                        createCounter(this, Counters.Time);
                    }
                },
                "level":
                {
                    name: "Level counter",
                    callback: function() {
                        createCounter(this, Counters.Level);
                    }
                },
                "loyality":
               {
                   name: "Loyality counter",
                   callback: function () {
                       createCounter(this, Counters.Loyality);
                   }
               },
            }
        },
        "move": {
            name: "Move to",
            items: {
                "top": {
                    name: "Top of the deck",
                    callback: function() {
                        moveTo(this, WhereToMove.Top, MoveFrom.Battlefield);
                    }
                },
                "bottom": {
                    name: "Bottom of the deck",
                    callback: function() {
                        moveTo(this, WhereToMove.Bottom, MoveFrom.Battlefield);
                    }
                }
            }
        },
        "Flip card": {
            name: "Flip",
            callback: function() {
                flip(this);
            }
        },
    }
});


$.contextMenu({
    selector: '.card.inHand',
    items: {
        "info": {
            name: "Info",
            callback: function () {
                prepareInfoModal(this);
            }
        },
        "move": {
            name: "Move to",
            items: {
                "top": {
                    name: "Top of the deck",
                    callback: function () {
                        moveTo(this, WhereToMove.Top, MoveFrom.Hand);
                    }
                },
                "bottom": {
                    name: "Bottom of the deck",
                    callback: function () {
                        moveTo(this, WhereToMove.Bottom, MoveFrom.Hand);
                    }
                }
            }
        }
    }
});