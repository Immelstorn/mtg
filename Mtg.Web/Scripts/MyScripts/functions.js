jQuery.fn.swap = function (b) {
    // method from: http://blog.pengoworks.com/index.cfm/2008/9/24/A-quick-and-dirty-swap-method-for-jQuery
    b = jQuery(b)[0];
    var a = this[0];
    var t = a.parentNode.insertBefore(document.createTextNode(''), a);
    b.parentNode.insertBefore(a, b);
    t.parentNode.insertBefore(b, t);
    t.parentNode.removeChild(t);
    return this;
};

var onAjaxSend = function () {
    activeAjaxRequests++;
    if (activeAjaxRequests > 0) {
        $('#ajaxLoader').addClass('active');
    }
}

var onAjaxComplete = function() {
    activeAjaxRequests--;
    if (activeAjaxRequests <= 0) {
        $('#ajaxLoader').removeClass('active');
    }
}

var changeLife = function (inc) {
    $.ajax({
        url: "/Play/ChangeLife",
        method: "POST",
        data: {
            "increment": inc,
        },
    }).done(function(result) {
        if (!result.Failed) {
            var value = $(".progress-label").text();
            if (inc) {
                value++;
            } else {
                value--;
            }
            $("#progressbar").progressbar("value", value);
            $('.progress-label').text(value);
        } else {
            showError(result.Error);
        }
    });
};

var tapUntap = function (elem) {
    var img = $(elem);
    var tap = img.attr('tap');
    var flip = img.attr('flip');

    if (tap != 'tap' && flip != 'flip') {
        img.rotate(90);
        img.attr('tap', 'tap');
    } else if (tap == 'tap' && flip != 'flip') {
        img.rotate(0);
        img.removeAttr('tap');
    } else if (tap != 'tap' && flip == 'flip') {
        img.rotate(270);
        img.attr('tap', 'tap');
    } else if (tap == 'tap' && flip == 'flip') {
        img.rotate(180);
        img.removeAttr('tap');
    }
    return false;
};

var flip = function (elem) {
    var img = $(elem);
    var tap = img.attr('tap');
    var flip = img.attr('flip');

    if (tap != 'tap' && flip != 'flip') {
        img.rotate(180);
        img.attr('flip', 'flip');
    } else if (tap == 'tap' && flip != 'flip') {
        img.rotate(270);
        img.attr('flip', 'flip');
    } else if (tap != 'tap' && flip == 'flip') {
        img.rotate(0);
        img.removeAttr('flip');
    } else if (tap == 'tap' && flip == 'flip') {
        img.rotate(90);
        img.removeAttr('flip');
    }
    return false;
};

var prepareInfoModal = function (elem) {
    var self = $(elem);
    var desc =
        '<h3>Card text</h3>' +
            self.attr('alt') +
            '<br/><br/>' +
            '<h3>Rulings</h3>' +
            self.attr('data-rulings');

    $('#cardInfo .header').text(self.attr('data-cardname'));
    $('#cardInfo .ui.image img').attr("src", self.attr('src'));
    $('#cardInfo .description').html(desc);
    $('#cardInfo').modal('setting', 'closable', false).modal('show');
}

var moveTo = function (elem, where, from) {
    var self = $(elem);
    var id = self[0].id;

    $.ajax({
        url: "/Play/MoveTo",
        data: {
            "cardLinkId": id,
            "where": where,
            "from": from
        },
        method: "POST",
    }).done(function (result) {
        if (!result.Failed) {

            if (where != WhereToMove.NA) {
                $('.counter' + id).remove();
            }

            if (where ==  WhereToMove.Top || where == WhereToMove.Bottom) {
                self.remove();
            }

            if (from == MoveFrom.Top) {
                removeAllClasses($('#' + id));

                if (where == WhereToMove.Hand) {
                    $('#hand').append(self[0].outerHTML);
                    $('#' + id).addClass('inHand');

                } else if (where == WhereToMove.Graveyard) {
                    $('#grave .image').append(self[0].outerHTML);
                    $('#' + id).addClass('inGrave');
                    
                } else if (where == WhereToMove.Exile) {
                    $('#exile .image').append(self[0].outerHTML);
                    $('#' + id).addClass('inExile');
                }

                $('#' + id).attr('style', 'top:0;left:0;position: absolute;');
                $('#' + id).height($('#cardHeight').val());
                self.remove();
                refreshDraggble($('#' + id));
            }
        } else {
            showError(result.Error);
        }
    });
}

var showError = function (error) {
    $('#errorModal .content').text(error);
    $('#errorModal').modal('setting', 'closable', false).modal('show');
}

var removeAllClasses = function (elem) {
    elem.removeClass('inHand').removeClass('inGame').removeClass('inGrave').removeClass('inExile').removeClass('onTopOfTheDeck');
}

var cardPositionChanged = function(percentOffset, id) {
    $.ajax({
        url: "/Play/CardPostitionChanged",
        data: {
            "cardLinkId": id,
            "top": percentOffset.top,
            "left": percentOffset.left
        },
        method: "POST",
    }).done(function(result) {
        if (result.Failed) {
            showError(result.Error);
        }
    });
}

var handleCardDrop = function(event, ui) {
    var self = $(this);
    if (self[0].id == 'grave') {
        if (!ui.draggable.hasClass('inGrave')) {
            ui.draggable.position({ of: self, my: 'center middle', at: 'center middle' });
            moveTo(ui.draggable[0], WhereToMove.Graveyard, MoveFrom.NA);
            removeAllClasses(ui.draggable);
            ui.draggable.addClass('inGrave');
        }
    } else if (self[0].id == 'exile') {
        if (!ui.draggable.hasClass('inExile')) {
            ui.draggable.position({ of: self, my: 'center middle', at: 'center middle' });
            moveTo(ui.draggable[0], WhereToMove.Exile, MoveFrom.NA);
            removeAllClasses(ui.draggable);
            ui.draggable.addClass('inExile');
        }
    } else if (self[0].id == 'hand') {
        if (!ui.draggable.hasClass('inHand')) {
            moveTo(ui.draggable[0], WhereToMove.Hand, MoveFrom.NA);
            removeAllClasses(ui.draggable);
            ui.draggable.addClass('inHand');
        }
    } else if (self[0].id == 'myBattlefield') {
        if (!ui.draggable.hasClass('inGame')) {
            moveTo(ui.draggable[0], WhereToMove.Battlefield, MoveFrom.NA);
            removeAllClasses(ui.draggable);
            ui.draggable.addClass('inGame');
        }


        var selfTop = self.offset().top;
        var selfLeft = self.offset().left;
        var selfHeight = self.height();
        var selfWidth = self.width();

        var cardTop = selfTop > ui.offset.top ? 0 : ui.offset.top - selfTop;
        var cardLeft = selfLeft > ui.offset.left ? 0 : ui.offset.left - selfLeft;
        var percentOffset = {
            top: cardTop / selfHeight,
            left: cardLeft / selfWidth,
        };
        cardPositionChanged(percentOffset, ui.draggable[0].id);

    }

    //if token is moved somewhere - it should be destroyed
    if (ui.draggable.hasClass('token') && !ui.draggable.hasClass('inGame')) {
        ui.draggable.remove();
    }
};

var handleCardOutOfDrop = function(event, ui) {
    var self = $(this);

    //token should not trigger these events
    if (ui.draggable.hasClass('token')) {
        return;
    }
    if (self[0].id == 'grave') {
        if (ui.draggable.hasClass('inGrave')) {
            moveTo(ui.draggable[0], WhereToMove.NA, MoveFrom.Graveyard);
            ui.draggable.removeClass('inGrave');
        }
    } else if (self[0].id == 'exile') {
        if (ui.draggable.hasClass('inExile')) {
            moveTo(ui.draggable[0], WhereToMove.NA, MoveFrom.Exile);
            ui.draggable.removeClass('inExile');
        }
    } else if (self[0].id == 'hand') {
        if (ui.draggable.hasClass('inHand')) {
            moveTo(ui.draggable[0], WhereToMove.NA, MoveFrom.Hand);
            ui.draggable.removeClass('inHand');
        }
    } else if (self[0].id == 'myBattlefield') {
        if (ui.draggable.hasClass('inGame')) {
            moveTo(ui.draggable[0], WhereToMove.NA, MoveFrom.Battlefield);
            ui.draggable.removeClass('inGame');
        }
    }
};

var escapeHtml = function (string) {
    var entityMap = {
        "&": "&amp;",
        "<": "&lt;",
        ">": "&gt;",
        '"': '&quot;',
        "'": '&#39;',
        "/": '&#x2F;'
    };
    return String(string).replace(/[&<>"'\/]/g, function (s) {
        return entityMap[s];
    });
};

var drawACard = function () {
    $.ajax({
        url: "/Play/Draw",
        method: "POST"
    }).done(function (card) {
        if (!card.Failed) {
            var height = $('#cardHeight').val();
            var html = prepareCardHtml(card.Data, card.Data.cardLinkId, true);
            $('#hand').append(html);
            $('#' + card.Data.cardLinkId).addClass('inHand');
            $('#' + card.Data.cardLinkId).height(height);
            refreshDraggble($('#' + card.Data.cardLinkId));
        } else {
            showError(card.Error);
        }
    });
}

var lookAtTheTop = function (value) {
    var count = parseInt(value);
    if (!isNaN(count)) {
        viewCards(ViewFrom.TopOfTheDeck, count);
        $('#changeOrderBtn').show();
        $('#deck').contextMenu("hide");
    }
}

var viewCards = function (from, count) {
    $.ajax({
        url: "/Play/ViewCards",
        data: {
            "from": from,
            "count": count
        },
        method: "POST",
    }).done(function (result) {
        if (!result.Failed) {

            var header = 'Cards ';
            var additionalClass = '';
            switch (from) {
                case ViewFrom.Graveyard:
                    header += 'at graveyard';
                    additionalClass = 'inGrave';
                    break;
                case ViewFrom.Exile:
                    header += 'in exile';
                    additionalClass = 'inExile';
                    break;
                case ViewFrom.EnemyHand:
                    header += "in enemy's hand";
                    additionalClass = 'inEnemyHand';
                    break;
                case ViewFrom.TopOfTheDeck:
                    header += "on top of the deck (bottom to top)";
                    additionalClass = 'onTopOfTheDeck';
                    break;
            }

            var html = '';
            for (var i = 0; i < result.Data.length; i++) {
                html += prepareCardHtml(result.Data[i], result.Data[i].cardLinkId, false);
            }
            $('#viewCardsModal .content .images').html(html);
            $('#viewCardsModal .content .images img').addClass(additionalClass);
            $('#viewCardsModal .header').text(header);
            $('#viewCardsModal').modal('setting', 'closable', false).modal('show');
        } else {
            showError(result.Error);
        }
    });
}

var cardOutOfHand = function () {

}

var prepareCardHtml = function (card, cardLinkId, fixPosition) {
    var rulings = '';
    if (card.rulings != null && card.rulings.length > 0) {
        for (var j = 0; j < card.rulings.length; j++) {
            rulings = rulings + card.rulings[j].date + ': ' + escapeHtml(card.rulings[j].text) + '<br/>';
        }
    }
    var html = '<img ' +
        'src="' + card.ImgLink + '' + '" ' +
        'alt="' + (card.text == "null" ? "" : escapeHtml(card.text)) + '" ' +
        'id="' + cardLinkId + '" ' +
        'class="card"' +
        'data-rulings="' + rulings + '" ' +
        'data-cardname="' + card.name + '" ' +
        '' + (fixPosition ? 'style="top:0;left:0;position: absolute;"' : '') + '' +
        '>';
  
    return html;
}

var compareCardsByNumber = function (a, b) {
    if (a.Number < b.Number)
        return -1;
    if (a.Number > b.Number)
        return 1;
    return 0;
}

var loadGame = function () {
    $.ajax({
        url: "/Play/GameInfo",
        method: "POST"
    }).done(function(game) {
        if (!game.Failed) {
            var enemyHp = game.Data.Result.AmIFirstPlayer ? game.Data.Result.SecondPlayerHp : game.Data.Result.FirstPlayerHp;
            var myHp = game.Data.Result.AmIFirstPlayer ? game.Data.Result.FirstPlayerHp : game.Data.Result.SecondPlayerHp;
            var enemyName = game.Data.Result.AmIFirstPlayer
                ? game.Data.Result.SecondPlayer == null ? "Opponent" : game.Data.Result.SecondPlayer.UserName
                : game.Data.Result.FirstPlayer == null ? "Opponent: " : game.Data.Result.FirstPlayer.UserName;

            document.title = enemyName + ' - Magic: The Gathering';
            $('.oponentName').text(enemyName);

            $('#enemyCards .label').text(game.Data.Result.EnemyCards);
            $('#enemyHp .label').text(enemyHp);
            $("#progressbar").progressbar({
                max: 20,
                value: myHp
            });
            $("#progressbar .progress-label").text(myHp);

            var grave = game.Data.Result.Grave.sort(compareCardsByNumber);
            var html = '';
            for (var i = 0; i < grave.length; i++) {
                html += prepareCardHtml(grave[i].CardLink.Card, grave[i].CardLink.Id, true);
            }
            $('#grave .image').append(html);
            $('#grave .image img.card').addClass('inGrave');


            var exile = game.Data.Result.Exile.sort(compareCardsByNumber);
            html = '';
            for (var j = 0; j < exile.length; j++) {
                html += prepareCardHtml(exile[j].CardLink.Card, exile[j].CardLink.Id, true);
            }

            $('#exile .image').append(html);
            $('#exile .image img.card').addClass('inExile');

            var bf = game.Data.Result.BattlefieldCards;
            html = '';
            for (var q = 0; q < bf.length; q++) {
                html += prepareCardHtml(bf[q].CardLink.Card, bf[q].CardLink.Id, true);
            }
            $('#myBattlefield').append(html);
            $('#myBattlefield img.card').addClass('inGame');

            for (var qq = 0; qq < bf.length; qq++) {
                var topPercent = bf[qq].Top;
                var leftPercent = bf[qq].Left;
                var id = bf[qq].CardLink.Id;
                var bfHeight = $('#myBattlefield').height();
                var bfWidth = $('#myBattlefield').width();
                $('#' + id).css('top', topPercent * bfHeight);
                $('#' + id).css('left', leftPercent * bfWidth);
            }

            var hand = game.Data.Result.HandCards;
            var height = $('#hand').height();
            var width = height * 0.7;
            var top = 2;
            var row = 0;
            var cardInARow = 0;

            $('#cardHeight').val(height);
            var handWidth = $('#hand').width();
            html = '';
            for (var z = 0; z < hand.length; z++) {
                var prepared = prepareCardHtml(hand[z].CardLink.Card, hand[z].CardLink.Id, false);
                var left = 2 + (width + 2) * cardInARow;
                if ((left + width) > handWidth) {
                    row++;
                    top = (height + 2) * row;
                    left = 2;
                    cardInARow = 0;
                }
                var elem = $(prepared).width(width).css('left', left).css('top', top);
                html += elem[0].outerHTML;
                cardInARow++;
            }

            $('#hand').height((height +2) * (row+1));

            $('#hand').append(html);
            $('.card').height(height);
            $('#hand .card:not(.inHand)').addClass('inHand');
            refreshDraggble(null);

            $('#grave img.card').one("load", function(e) {
                $(e.target).position({ of: $('#grave'), my: 'center middle', at: 'center middle' });
            });

            $('#exile img.card').one("load", function (e) {
                $(e.target).position({ of: $('#exile'), my: 'center middle', at: 'center middle' });
            });

        } else {
            showError(game.Error);
        }

    });
}

var onDOMAttrModified = function(e) {
    if (e.originalEvent.attrName == 'style') {
        var prevValue = e.originalEvent.prevValue.match('z-index: ([0-9]*);');
        var newValue = e.originalEvent.newValue.match('z-index: ([0-9]*);');
        if (prevValue != null && newValue != null && prevValue[1] != newValue[1]) {
            var id = e.target.id;

            $('.counter' + id).each(function() {
                $(this).zIndex(parseInt(newValue[1]));
            });
        }
    }
}

var refreshDraggble = function(card) {
    var target = card == null ? $('.card') : card;

    target.draggable({
        stack: ".card",
        containment: '#mainGrid',
        revert: "invalid",
        stop: function(e, ui) {
            moveCounters(ui);
        }
    }).css("position", "absolute");

    target.on("DOMAttrModified", onDOMAttrModified);
}

var moveCounters = function(ui) {
    var id = ui.helper[0].id;

    $('.counter' + id).each(function() {
        var self = $(this);
        var oldPosition = self.offset();
        var leftDelta = ui.position.left - ui.originalPosition.left;
        var topDelta = ui.position.top - ui.originalPosition.top;

        self.offset({
            top: oldPosition.top + topDelta,
            left: oldPosition.left + leftDelta
        });
        self.zIndex(ui.helper.zIndex() + 1);
    });
}

var removeCounter = function(elem) {
    var counter = $(elem);
    counter.remove();
}

var createCounter = function(elem, counterType) {
    var card = $(elem);

    var typeClass = '';
    var typeText = '';
    switch (counterType) {
    case Counters.Plus1:
        typeClass = 'plus1';
        typeText = '+1/+1';
        break;
    case Counters.Minus1:
        typeClass = 'minus1';
        typeText = '-1/-1';
        break;
    case Counters.Fade:
        typeClass = 'fade';
        typeText = 'Fade';
        break;
    case Counters.Time:
        typeClass = 'time';
        typeText = 'Time';
        break;
    case Counters.Level:
        typeClass = 'level';
        typeText = 'Level';
        break;
    case Counters.Loyality:
        typeClass = 'loyality';
        typeText = 'Loyality';
        break;

    }

    var html = '<div class="counter counter' + card[0].id + ' ' + typeClass + '">'+typeText+'</div>';

    var counter = $(html).appendTo(card.parent());
    counter.height(card.height() * 0.2);
    counter.width(card.height() * 0.2);

    var offset = card.offset();
    counter.offset({ top: offset.top, left: offset.left });
    counter.zIndex(card.zIndex() + 1);

    $('.counter' + card[0].id).draggable({
        stack: '.counter' + card[0].id,
        containment: '#' + card[0].id
    }).css("position", "absolute");
}

var changeOrderOfCardsCallback = function() {
    
    var cards = $('#viewCardsModal .content .images img');
    var order = [];

    for (var i = 0; i < cards.length; i++) {
        order.push(cards[i].id);
    }

    $.ajax({
        url: "/Play/ChangeOrder",
        method: "POST",
        data: {
            "order": order,
        },
    }).done(function (result) {
        if (!result.Failed) {

        } else {
            showError(result.Error);
        }
    });

    $('#viewCardsModal').modal('setting', 'onApprove', function() {});
}

var changeOrderOfCards = function() {
    var cards = $('#viewCardsModal .content .images img');
    $('#viewCardsModal .header').html($('#viewCardsModal .header').html() + '  <h5>order changing mode enabled</h5>');
    $('#changeOrderBtn').hide();
    $('#viewCardsModal').modal('setting', 'onApprove', changeOrderOfCardsCallback);

    cards.draggable({ helper: "clone" });

    cards.droppable({
        accept: ".onTopOfTheDeck",
        activeClass: "ui-state-hover",
        hoverClass: "ui-state-active",
        drop: function(event, ui) {

            var draggable = ui.draggable,
                droppable = $(this),
                dragPos = draggable.position(),
                dropPos = droppable.position();

            draggable.css({
                left: dropPos.left + 'px',
                top: dropPos.top + 'px'
            });

            droppable.css({
                left: dragPos.left + 'px',
                top: dragPos.top + 'px'
            });
            draggable.swap(droppable);
        }
    });
}

var addToken = function() {
    $('#addToken').modal('setting', 'onApprove', createToken).modal('show');
    $('#addTokenColor').dropdown();
}

var createToken = function() {
    $('#addToken .ui.form').form('validate form');
    if (!$('#addToken .form').form('is valid')) {
        console.log(false);
        return false;
    }

    var name = $('#addTokenName').val();
    var type = $('#addTokenType').val();
    var color = $('#addTokenColor').val();
    var text = $('#addTokenText').val();
    var power = $('#addTokenPower').val();
    var toughness = $('#addTokenToughness').val();

    $.ajax({
        url: "/Play/CreateToken",
        method: "POST",
        data: {
            "name": name,
            "type": type,
            "color": color,
            "text": text,
            "power": power,
            "toughness": toughness,
        },
    }).done(function (result) {
        if (!result.Failed) {

            var height = $('#cardHeight').val();
            var html = prepareCardHtml(result.Data.Token, result.Data.Id, true);
            $('#hand').append(html);
            $('#' + result.Data.Id).addClass('inGame');
            $('#' + result.Data.Id).addClass('token');
            $('#' + result.Data.Id).height(height);
            refreshDraggble($('#' + result.Data.Id));
        } else {
            showError(result.Error);
        }
    });
    return true;
}