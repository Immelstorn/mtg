$('#grave').droppable({
    activeClass: "custom-state-active",
    drop: handleCardDrop,
    out: handleCardOutOfDrop
});

$('#exile').droppable({
    activeClass: "custom-state-active",
    drop: handleCardDrop,
    out: handleCardOutOfDrop
});

$('#myBattlefield').droppable({
    activeClass: "custom-state-active",
    drop: handleCardDrop,
    out: handleCardOutOfDrop
});

$('#hand').droppable({
    activeClass: "custom-state-active",
    drop: handleCardDrop,
    out: handleCardOutOfDrop
});

$('.ui.modal').modal({
    allowMultiple: true,
    duration: 0,
    approve: '.ok'
});

$('#addToken .ui.form')
    .form({
        fields: {
            addTokenName: {
                rules: [
                    {
                        type: 'maxLength[50]',
                        prompt: 'Name: Maximum langth is 50'
                    }
                ]
            },
            addTokenText: {
                rules: [
                    {
                        type: 'maxLength[255]',
                        prompt: 'Text: Maximum langth is 255'
                    }
                ]
            },
            addTokenType: {
                rules: [
                    {
                        type: 'empty',
                        prompt: 'Type: This field is required'
                    },
                    {
                        type: 'maxLength[50]',
                        prompt: 'Type: Maximum langth is 50'
                    }
                ]
            },
            addTokenPower: {
                rules: [
                    {
                        type: 'integer[0..99]',
                        prompt: 'Power: Only integer 0..99 is allowed'
                    },
                    {
                        type: 'empty',
                        prompt: 'Power: This field is required'
                    }
                ]
            },
            addTokenToughness: {
                rules: [
                    {
                        type: 'integer[1..99]',
                        prompt: 'Thougness: Only integer 1..99 is allowed'
                    },
                    {
                        type: 'empty',
                        prompt: 'Thougness: This field is required'
                    }
                ]
            }
        }
    });