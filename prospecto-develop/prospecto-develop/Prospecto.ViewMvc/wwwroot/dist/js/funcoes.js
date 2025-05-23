/*----------Plugin de Mask, Validações, Converções, ReplaceAll----------*/
(function (fn, $) {
    /*----------Masks----------*/
    $.fn.maskInteiro = function () {
        $(this).keypress(function (e) {
            if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57))
                return false;
            else
                return true;
        });
    };
    $.fn.maskDecimal = function () {
        $(this).keypress(function (e) {
            var numero = $(this).val();
            if (e.which != 44 && e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                return false;
            } else {
                if (numero.search(',') != -1 && e.which == 44) {
                    return false;
                } else {
                    if (numero.search('0') == 0 && numero.length == 1 && e.which == 48) {
                        return false;
                    } else {
                        return true;
                    }
                }
            }
        });
        $(this).focus(function () {
            $(this).val($(this).val().replaceAll('.', ''));
        });
        $(this).blur(function () {
            $(this).val($(this).val().toDecimalMask());
        });
    };
    $.fn.maskData = function () {
        $(this).mask("99/99/9999");
    };
    $.fn.maskCep = function () {
        $(this).mask("99999-999");
    };
    $.fn.maskCpf = function () {
        $(this).mask("999.999.999-99");
    };
    $.fn.maskCnpj = function () {
        $(this).mask("99.999.999/9999-99");
    };
    $.fn.maskTelefone = function () {
        $(this).keypress(function (e) {
            if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57) && e.which != 40 && e.which != 41 && e.which != 45 && e.which != 43)
                return false;
            else
                return true;
        });
    };
    /*----------Masks----------*/


    /*----------Validações----------*/
    window.isInt = function (inteiro) {
        if ((parseFloat(inteiro) == parseInt(inteiro)) && !isNaN(inteiro))
            return true;
        else
            return false;
    };
    window.isString = function (string) {
        string = string.replaceAll(' ', '');
        if (string != "")
            return true;
        else
            return false;
    };
    window.isDecimal = function (decimal) {
        if (decimal != "" && !isNaN(decimal))
            return true;
        else
            return false;
    };
    window.isDate = function (data) {
        var dia = (data.substring(0, 2));
        var mes = (data.substring(3, 5));
        var ano = (data.substring(6, 10));

        if ((dia < 01) || (dia < 01 || dia > 30) && (mes == 04 || mes == 06 || mes == 09 || mes == 11) || dia > 31) {
            return false;
        }

        if (mes < 01 || mes > 12) {
            return false;
        }

        if (mes == 2 && (dia < 01 || dia > 29 || (dia > 28 && (parseInt(ano / 4) != ano / 4)))) {
            return false;
        }

        if (data.value == "") {
            return false;
        }
        return true;
    };
    window.isEmail = function (email) {
        var re = /^[^@]+@[^@]+.[a-z]{2,}$/i;
        if (email.search(re) == -1)
            return false;
        else {
            return true;
        }
    };
    window.isCep = function (cep) {
        var objEr = /^[0-9]{5}-[0-9]{3}$/;

        if (cep.length > 0) {
            if (objEr.test(cep))
                return true;
            else
                return false;
        } else {
            return false;
        }
    };
    window.isCpf = function (cpf) {
        cpf = cpf.replace('.', '').replace('.', '').replace('-', '');

        if (cpf.length != 11 || cpf == "00000000000" || cpf == "11111111111" || cpf == "22222222222" || cpf == "33333333333" || cpf == "44444444444" || cpf == "55555555555" || cpf == "66666666666" || cpf == "77777777777" || cpf == "88888888888" || cpf == "99999999999") {
            return false;
        }

        var soma = 0;

        for (var i = 0; i < 9; i++) {
            soma += parseInt(cpf.charAt(i)) * (10 - i);
        }

        var rev = 11 - (soma % 11);

        if (rev == 10 || rev == 11) {
            rev = 0;
        }

        if (rev != parseInt(cpf.charAt(9))) {
            return false;
        }

        soma = 0;

        for (i = 0; i < 10; i++) {
            soma += parseInt(cpf.charAt(i)) * (11 - i);
        }

        rev = 11 - (soma % 11);

        if (rev == 10 || rev == 11)
            rev = 0;

        if (rev != parseInt(cpf.charAt(10))) {
            return false;
        }

        return true;
    };
    window.isCnpj = function (cnpj) {
        var valida = new Array(6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2);
        var dig1 = new Number;
        var dig2 = new Number;

        var exp = /\.|\-|\//g;
        cnpj = cnpj.toString().replace(exp, "");

        var digito = new Number(eval(cnpj.charAt(12) + cnpj.charAt(13)));

        for (var i = 0; i < valida.length; i++) {
            dig1 += (i > 0 ? (cnpj.charAt(i - 1) * valida[i]) : 0);
            dig2 += cnpj.charAt(i) * valida[i];
        }

        dig1 = (((dig1 % 11) < 2) ? 0 : (11 - (dig1 % 11)));
        dig2 = (((dig2 % 11) < 2) ? 0 : (11 - (dig2 % 11)));

        if (((dig1 * 10) + dig2) != digito) {
            return false;
        }

        return true;
    };
    /*----------Validações----------*/


    /*----------Converções de Objetos----------*/
    $.fn.toInt = function () {
        return $(this).val().toInt();
    };
    $.fn.toDecimal = function () {
        return $(this).val().toDecimal();
    };
    $.fn.toDecimalMask = function () {
        return $(this).val().toDecimalMask();
    };
    /*----------Converções de Objetos----------*/


    /*----------Converções de String----------*/
    String.prototype.toInt = function () {
        return parseInt(isNaN(this) ? 0 : this);
    };
    String.prototype.toDecimal = function () {
        var decimal = this.toString().replaceAll('.', '').replace(',', '.');
        return parseFloat(isNaN(decimal) ? 0 : decimal);
    };
    String.prototype.toDecimalMask = function () {
        var numero = this;
        var inteiro = numero.split(',')[0].replaceAll('.', '');
        var decimal = numero.split(',')[1];

        //Inteiro
        var inteiro2 = inteiro;
        for (var f = 1; f < inteiro.length; f++) {
            if (inteiro2.charAt(0) == 0) {
                inteiro2 = inteiro2.slice(1);
            } else {
                f = inteiro.length;
            }
        }
        if (inteiro2 == 0) {
            inteiro2 = 0;
        }

        var qtdInteiros = inteiro2.length;
        var inteiro3;

        if (qtdInteiros > 3) {
            if (qtdInteiros % 3 > 0) {
                inteiro3 = inteiro2.slice(0, qtdInteiros % 3) + '.';
                for (var k = qtdInteiros % 3; k < qtdInteiros; k += 3) {
                    inteiro3 += inteiro2.slice(k, k + 3);
                    if (k + 3 < qtdInteiros) {
                        inteiro3 += '.';
                    }
                }
            } else {
                inteiro3 = '';
                for (var l = qtdInteiros % 3; l < qtdInteiros; l += 3) {
                    inteiro3 += inteiro2.slice(l, l + 3);
                    if (l + 3 < qtdInteiros) {
                        inteiro3 += '.';
                    }
                }
            }
        } else {
            inteiro3 = inteiro2;
        }

        //Decimal
        var decimal2 = decimal;
        if (decimal) {
            for (var i = 1; i < decimal.length; i++) {
                if (decimal2.charAt(decimal.length - i) == 0) {
                    decimal2 = decimal2.slice(0, -1);
                } else {
                    i = decimal.length;
                }
            }
            if (decimal2.length < 2) {
                decimal2 += 0;
            }
        } else {
            decimal2 = '00';
        }

        return (inteiro3 + ',' + decimal2);
    };
    /*----------Converções de String----------*/


    /*----------Outras----------*/
    String.prototype.replaceAll = function (from, to) {
        var str = this;
        var pos = str.indexOf(from);
        while (pos > -1) {
            str = str.replace(from, to);
            pos = str.indexOf(from);
        }
        return (str);
    };
    /*----------Outras----------*/
})(window.fn = window.fn || {}, jQuery);
/*----------Plugin de Mask, Validações, Converções, ReplaceAll----------*/


/*----------Plugin de Mask----------*/
(function ($) {
    function getPasteEvent() {
        var el = document.createElement('input'),
            name = 'onpaste';
        el.setAttribute(name, '');
        return (typeof el[name] === 'function') ? 'paste' : 'input';
    }
    var pasteEventName = getPasteEvent() + ".mask",
            ua = navigator.userAgent,
            iPhone = /iphone/i.test(ua),
            chrome = /chrome/i.test(ua),
            android = /android/i.test(ua),
            caretTimeoutId;

    $.mask = {
        //Predefined character definitions
        definitions: {
            '9': "[0-9]",
            'a': "[A-Za-z]",
            '*': "[A-Za-z0-9]"
        },
        autoclear: true,
        dataName: "rawMaskFn",
        placeholder: '_'
    };
    $.fn.extend({
        //Helper Function for Caret positioning
        caret: function (begin, end) {
            var range;

            if (this.length === 0 || this.is(":hidden")) {
                return;
            }

            if (typeof begin == 'number') {
                end = (typeof end === 'number') ? end : begin;
                return this.each(function () {
                    if (this.setSelectionRange) {
                        this.setSelectionRange(begin, end);
                    } else if (this.createTextRange) {
                        range = this.createTextRange();
                        range.collapse(true);
                        range.moveEnd('character', end);
                        range.moveStart('character', begin);
                        range.select();
                    }
                });
            } else {
                if (this[0].setSelectionRange) {
                    begin = this[0].selectionStart;
                    end = this[0].selectionEnd;
                } else if (document.selection && document.selection.createRange) {
                    range = document.selection.createRange();
                    begin = 0 - range.duplicate().moveStart('character', -100000);
                    end = begin + range.text.length;
                }
                return { begin: begin, end: end };
            }
        },
        unmask: function () {
            return this.trigger("unmask");
        },
        mask: function (mask, settings) {
            var input,
                    defs,
                    tests,
                    partialPosition,
                    firstNonMaskPos,
                    len;

            if (!mask && this.length > 0) {
                input = $(this[0]);
                return input.data($.mask.dataName)();
            }
            settings = $.extend({
                autoclear: $.mask.autoclear,
                placeholder: $.mask.placeholder, // Load default placeholder
                completed: null
            }, settings);


            defs = $.mask.definitions;
            tests = [];
            partialPosition = len = mask.length;
            firstNonMaskPos = null;

            $.each(mask.split(""), function (i, c) {
                if (c == '?') {
                    len--;
                    partialPosition = i;
                } else if (defs[c]) {
                    tests.push(new RegExp(defs[c]));
                    if (firstNonMaskPos === null) {
                        firstNonMaskPos = tests.length - 1;
                    }
                } else {
                    tests.push(null);
                }
            });

            return this.trigger("unmask").each(function () {
                var input = $(this),
                        buffer = $.map(
                        mask.split(""),
                        function (c, i) {
                            if (c != '?') {
                                return defs[c] ? settings.placeholder : c;
                            }
                        }),
                        defaultBuffer = buffer.join(''),
                        focusText = input.val();

                function seekNext(pos) {
                    while (++pos < len && !tests[pos]);
                    return pos;
                }

                function seekPrev(pos) {
                    while (--pos >= 0 && !tests[pos]);
                    return pos;
                }

                function shiftL(begin, end) {
                    var i,
                            j;

                    if (begin < 0) {
                        return;
                    }

                    for (i = begin, j = seekNext(end) ; i < len; i++) {
                        if (tests[i]) {
                            if (j < len && tests[i].test(buffer[j])) {
                                buffer[i] = buffer[j];
                                buffer[j] = settings.placeholder;
                            } else {
                                break;
                            }

                            j = seekNext(j);
                        }
                    }
                    writeBuffer();
                    input.caret(Math.max(firstNonMaskPos, begin));
                }

                function shiftR(pos) {
                    var i,
                            c,
                            j,
                            t;

                    for (i = pos, c = settings.placeholder; i < len; i++) {
                        if (tests[i]) {
                            j = seekNext(i);
                            t = buffer[i];
                            buffer[i] = c;
                            if (j < len && tests[j].test(t)) {
                                c = t;
                            } else {
                                break;
                            }
                        }
                    }
                }

                function blurEvent(e) {
                    checkVal();

                    if (input.val() != focusText)
                        input.change();
                }

                function keydownEvent(e) {
                    var k = e.which,
                            pos,
                            begin,
                            end;

                    //backspace, delete, and escape get special treatment
                    if (k === 8 || k === 46 || (iPhone && k === 127)) {
                        pos = input.caret();
                        begin = pos.begin;
                        end = pos.end;

                        if (end - begin === 0) {
                            begin = k !== 46 ? seekPrev(begin) : (end = seekNext(begin - 1));
                            end = k === 46 ? seekNext(end) : end;
                        }
                        clearBuffer(begin, end);
                        shiftL(begin, end - 1);

                        e.preventDefault();
                    } else if (k === 13) { // enter
                        blurEvent.call(this, e);
                    } else if (k === 27) { // escape
                        input.val(focusText);
                        input.caret(0, checkVal());
                        e.preventDefault();
                    }
                }

                function keypressEvent(e) {
                    var k = e.which,
                            pos = input.caret(),
                            p,
                            c,
                            next;

                    if (k == 0) {
                        // unable to detect key pressed. Grab it from pos and adjust
                        // this is a failsafe for mobile chrome
                        // which can't detect keypress events
                        // reliably
                        if (pos.begin >= len) {
                            input.val(input.val().substr(0, len));
                            e.preventDefault();
                            return false;
                        }
                        if (pos.begin == pos.end) {
                            k = input.val().charCodeAt(pos.begin - 1);
                            pos.begin--;
                            pos.end--;
                        }
                    }

                    if (e.ctrlKey || e.altKey || e.metaKey || k < 32) {//Ignore
                        return;
                    } else if (k && k !== 13) {
                        if (pos.end - pos.begin !== 0) {
                            clearBuffer(pos.begin, pos.end);
                            shiftL(pos.begin, pos.end - 1);
                        }

                        p = seekNext(pos.begin - 1);
                        if (p < len) {
                            c = String.fromCharCode(k);
                            if (tests[p].test(c)) {
                                shiftR(p);

                                buffer[p] = c;
                                writeBuffer();
                                next = seekNext(p);

                                if (android) {
                                    setTimeout($.proxy($.fn.caret, input, next), 0);
                                } else {
                                    input.caret(next);
                                }

                                if (settings.completed && next >= len) {
                                    settings.completed.call(input);
                                }
                            }
                        }
                        e.preventDefault();
                    }
                }

                function clearBuffer(start, end) {
                    var i;
                    for (i = start; i < end && i < len; i++) {
                        if (tests[i]) {
                            buffer[i] = settings.placeholder;
                        }
                    }
                }

                function writeBuffer() { input.val(buffer.join('')); }

                function checkVal(allow) {
                    //try to place characters where they belong
                    var test = input.val(),
                            lastMatch = -1,
                            i,
                            c,
                            pos;

                    for (i = 0, pos = 0; i < len; i++) {
                        if (tests[i]) {
                            buffer[i] = settings.placeholder;
                            while (pos++ < test.length) {
                                c = test.charAt(pos - 1);
                                if (tests[i].test(c)) {
                                    buffer[i] = c;
                                    lastMatch = i;
                                    break;
                                }
                            }
                            if (pos > test.length) {
                                break;
                            }
                        } else if (buffer[i] === test.charAt(pos) && i !== partialPosition) {
                            pos++;
                            lastMatch = i;
                        }
                    }
                    if (allow) {
                        writeBuffer();
                    } else if (lastMatch + 1 < partialPosition) {
                        if (settings.autoclear || buffer.join('') === defaultBuffer) {
                            // Invalid value. Remove it and replace it with the
                            // mask, which is the default behavior.
                            input.val("");
                            clearBuffer(0, len);
                        } else {
                            // Invalid value, but we opt to show the value to the
                            // user and allow them to correct their mistake.
                            writeBuffer();
                        }
                    } else {
                        writeBuffer();
                        input.val(input.val().substring(0, lastMatch + 1));
                    }
                    return (partialPosition ? i : firstNonMaskPos);
                }

                input.data($.mask.dataName, function () {
                    return $.map(buffer, function (c, i) {
                        return tests[i] && c != settings.placeholder ? c : null;
                    }).join('');
                });

                if (!input.attr("readonly"))
                    input
                    .one("unmask", function () {
                        input
                                .unbind(".mask")
                                .removeData($.mask.dataName);
                    })
                    .bind("focus.mask", function () {
                        clearTimeout(caretTimeoutId);
                        var pos;

                        focusText = input.val();

                        pos = checkVal();

                        caretTimeoutId = setTimeout(function () {
                            writeBuffer();
                            if (pos == mask.replace("?", "").length) {
                                input.caret(0, pos);
                            } else {
                                input.caret(pos);
                            }
                        }, 10);
                    })
                    .bind("blur.mask", blurEvent)
                    .bind("keydown.mask", keydownEvent)
                    .bind("keypress.mask", keypressEvent)
                    .bind(pasteEventName, function () {
                        setTimeout(function () {
                            var pos = checkVal(true);
                            input.caret(pos);
                            if (settings.completed && pos == input.val().length)
                                settings.completed.call(input);
                        }, 0);
                    });
                if (chrome && android) {
                    input.bind("keyup.mask", keypressEvent);
                }
                checkVal(); //Perform initial check for existing values
            });
        }
    });
})(jQuery);
/*----------Plugin de Mask----------*/