
function consultaTabulado() {
    //////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////

    var modelo;
    var fuente;
    var variables = "";
    var actividades = "";
    var tipoDato;
    var anios;
    fuente = 1;
    modelo = 1;
    $("#Desglose :checked").each(function () {
        console.log($(this).val())
        if (variables != "") {
            variables = variables + ","
        }
        variables = variables + $(this).val();
    })
    $("#DesgloseActs :checked").each(function () {
        console.log($(this).val())
        if (variables != "") {
            actividades = actividades + ","
        }
        actividades = actividades + $(this).val();
    })
    tipoDato = "1";
    anios = "2014";
    //////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////


    $.ajax({
        type: "POST",
        url: "servicioweb.asmx/getConsulta",
        data: "{'modelo':'" + modelo + "','fuente':'" + fuente + "','variable':'" + variables + "','tipoDato':'" + tipoDato + "','actividad':'" + actividades + "','anio':'" + anios + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var json = JSON.parse(msg.d);
            console.log(json);

            var varSelectedValor = new JSON.constructor();
            var actSelectedValor = new JSON.constructor();
            varSelectedValor.length = 0;
            actSelectedValor.length = 0;
            for (i = 0; i < json.length; i++) {
                var bandera = true;
                for (j = 0; j < varSelectedValor.length; j++) {
                    if (varSelectedValor[j] == json[i].IdVariable) {
                        bandera = false;
                    }
                }
                if (bandera) {
                    varSelectedValor[varSelectedValor.length] = json[i].IdVariable;
                    varSelectedValor.length++;
                }
                ////////////////////////////////////////////////////////
                bandera = true;
                for (j = 0; j < actSelectedValor.length; j++) {
                    if (actSelectedValor[j] == json[i].IdActividad) {
                        bandera = false;
                    }
                }
                if (bandera) {
                    actSelectedValor[actSelectedValor.length] = json[i].IdActividad;
                    actSelectedValor.length++;
                }
            }
            var jsonColspan = new JSON.constructor();
            for (v = 0; v < varSelectedValor.length; v++) {
                var strAux = ""
                for (subV = 0; subV < varSelectedValor[v].split("|").length; subV++) {
                    if (strAux != "") {
                        strAux = strAux + "|"
                    }
                    strAux = strAux + varSelectedValor[v].split("|")[subV];
                    if ( jsonColspan[strAux] == null) {
                        jsonColspan[strAux] = 1;
                    } else {
                        jsonColspan[strAux]++
                    }
                }

            }
            console.log(jsonColspan)








        }

    });



}