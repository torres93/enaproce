/// <reference path="../../servicioweb.asmx" />
var todasLasVariables;
var jVariables;
var varSelected;
var todasLasActividades;
var jActividades;
var actSelected;
$(document).ready(function () {
    consultaFuentes();
})
function consultaFuentes() {




    var modelo = $("#modelo").val();
    $.ajax({
        type: "POST",
        url: "servicioweb.asmx/getFuentes",
        data: "{'modelo':'" + modelo + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var jFuentes = JSON.parse(msg.d);
            var select = document.getElementById("fuente");
            if (select != null) {
                while (select.hasChildNodes()) {
                    select.removeChild(select.firstChild);
                }
            }
            for (x = 0; x < jFuentes.length; x++) {
                var option = document.createElement("option");

                option.label = jFuentes[x]["Descripcion"];
                option.innerHTML = jFuentes[x]["IdFuente"];
                select.appendChild(option);

            }


            init(select.value);

        }
    });
}
function init(fuente) {
    var modelo = $("#modelo").val();
    $.ajax({
        type: "POST",
        url: "servicioweb.asmx/getVariables",
        data: "{'modelo':'" + modelo + "','fuente':'" + fuente + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            jVariables = JSON.parse(msg.d);
            todasLasVariables = new JSON.constructor();


            for (var x = 0; x < jVariables.length; x++) {
                if (todasLasVariables[jVariables[x]["IdVariableCompuesta"]] == null) {
                    todasLasVariables[jVariables[x]["IdVariableCompuesta"]] = new JSON.constructor();
                    todasLasVariables[jVariables[x]["IdVariableCompuesta"]] = jVariables[x];
                    todasLasVariables[jVariables[x]["IdVariableCompuesta"]].length = 0;
                }
                if (todasLasVariables[jVariables[x]["IdVariablePadre"]] == null) {
                    todasLasVariables[jVariables[x]["IdVariablePadre"]] = new JSON.constructor();
                    todasLasVariables[jVariables[x]["IdVariablePadre"]][0] = new JSON.constructor();
                    todasLasVariables[jVariables[x]["IdVariablePadre"]][0] = jVariables[x];
                    todasLasVariables[jVariables[x]["IdVariablePadre"]].length++;
                } else {
                    if (todasLasVariables[jVariables[x]["IdVariablePadre"]]["IdVariable"] != jVariables[x]["IdVariable"]) {
                        todasLasVariables[jVariables[x]["IdVariablePadre"]][todasLasVariables[jVariables[x]["IdVariablePadre"]].length] = new JSON.constructor();
                        todasLasVariables[jVariables[x]["IdVariablePadre"]][todasLasVariables[jVariables[x]["IdVariablePadre"]].length] = jVariables[x];
                        todasLasVariables[jVariables[x]["IdVariablePadre"]].length++;
                    }
                }


            }
            var select = document.getElementById("selectVariables");

            if (select != null) {
                while (select.hasChildNodes()) {
                    select.removeChild(select.firstChild);
                }
            }
            for (x = 0; x < jVariables.length; x++) {
                if (jVariables[x].Nivel == 1) {
                    var option = document.createElement("option");

                    option.label = jVariables[x]["Descripcion"];
                    option.innerHTML = jVariables[x]["IdVariableCompuesta"];
                    select.appendChild(option);
                }
            }

            consultaDesglose(select.value)


        }
    });





    $.ajax({
        type: "POST",
        url: "servicioweb.asmx/getActividades",
        data: "{'modelo':'" + modelo + "','fuente':'" + fuente + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            jActividades = JSON.parse(msg.d);
            todasLasActividades = new JSON.constructor();


            for (var x = 0; x < jActividades.length; x++) {
                if (todasLasActividades[jActividades[x]["IdActividadCompuesta"]] == null) {
                    todasLasActividades[jActividades[x]["IdActividadCompuesta"]] = new JSON.constructor();
                    todasLasActividades[jActividades[x]["IdActividadCompuesta"]] = jActividades[x];
                    todasLasActividades[jActividades[x]["IdActividadCompuesta"]].length = 0;
                }
                if (todasLasActividades[jActividades[x]["IdActividadPadre"]] == null) {
                    todasLasActividades[jActividades[x]["IdActividadPadre"]] = new JSON.constructor();
                    todasLasActividades[jActividades[x]["IdActividadPadre"]][0] = new JSON.constructor();
                    todasLasActividades[jActividades[x]["IdActividadPadre"]][0] = jActividades[x];
                    todasLasActividades[jActividades[x]["IdActividadPadre"]].length++;
                } else {
                    if (todasLasActividades[jActividades[x]["IdActividadPadre"]]["IdActividad"] != jActividades[x]["IdActividad"]) {
                        todasLasActividades[jActividades[x]["IdActividadPadre"]][todasLasActividades[jActividades[x]["IdActividadPadre"]].length] = new JSON.constructor();
                        todasLasActividades[jActividades[x]["IdActividadPadre"]][todasLasActividades[jActividades[x]["IdActividadPadre"]].length] = jActividades[x];
                        todasLasActividades[jActividades[x]["IdActividadPadre"]].length++;
                    }
                }


            }
            var select = document.getElementById("selectActividades");

            if (select != null) {
                while (select.hasChildNodes()) {
                    select.removeChild(select.firstChild);
                }
            }
            for (x = 0; x < jActividades.length; x++) {
                if (jActividades[x].Nivel == 1) {
                    var option = document.createElement("option");

                    option.label = jActividades[x]["Descripcion"];
                    option.innerHTML = jActividades[x]["IdActividadCompuesta"];
                    select.appendChild(option);
                }
            }

            consultaDesgloseAct(select.value)


        }
    });








}


function consultaDesgloseAct(actividad) {
    actSelected = new JSON.constructor();
    actSelected.length = 0;
    var comboDesglose = document.getElementById("DesgloseActs");

    if (comboDesglose != null) {
        while (comboDesglose.hasChildNodes()) {
            comboDesglose.removeChild(comboDesglose.firstChild);
        }
    }
    comboDesglose.hidden = true;
    $('#DesgloseActs').checkboxTree();


    var actividad = document.getElementById("selectActividades").value;

    for (x = 0; x < jActividades.length; x++) {
        if (jActividades[x].Nivel > 1 && jActividades[x].IdActividadCompuesta.split("|")[0] == actividad) {

            comboDesglose.hidden = false;
            var li = document.createElement("li");
            var input = document.createElement("input");
            var label = document.createElement("label");
            li.id = "li" + jActividades[x].IdActividadCompuesta;
            input.type = "checkbox";
            input.setAttribute("class", li.id);
            input.setAttribute("onClick", "addActividadSelected('" + jActividades[x].IdActividadCompuesta + "','"+li.id+"')");
            label.innerHTML = jActividades[x].Descripcion;

            if (todasLasActividades[jActividades[x].IdActividadCompuesta].length > 0) {
                var span = document.createElement("span");
                span.setAttribute("class", "ui-icon ui-icon-triangle-1-e")
                span.id = "span" + jActividades[x].IdActividadCompuesta
                span.setAttribute("onclick", "expandeUL('" + jActividades[x].IdActividadCompuesta + "')");
                li.appendChild(span);
            }
            li.appendChild(input);
            li.appendChild(label);
            if (todasLasActividades[jActividades[x].IdActividadCompuesta].length > 0) {

                var ul = document.createElement("ul")
                ul.id = "ul" + jActividades[x].IdActividadCompuesta;
                ul.hidden = true;
                li.appendChild(ul);
            }


            if (jActividades[x].Nivel == 2) {
                comboDesglose.appendChild(li);
            } else {
                document.getElementById("ul" + jActividades[x].IdActividadPadre).appendChild(li);
            }
        }
    }


}


function consultaDesglose(variable) {
    varSelected = new JSON.constructor();
    varSelected.length = 0;
    var comboDesglose = document.getElementById("Desglose");

    if (comboDesglose != null) {
        while (comboDesglose.hasChildNodes()) {
            comboDesglose.removeChild(comboDesglose.firstChild);
        }
    }
    comboDesglose.hidden = true;
    $('#Desglose').checkboxTree();


    var variable = document.getElementById("selectVariables").value;

    for (x = 0; x < jVariables.length; x++) {
        if (jVariables[x].Nivel > 1 && jVariables[x].IdVariableCompuesta.split("|")[0] == variable) {

            comboDesglose.hidden = false;
            var li = document.createElement("li");
            var input = document.createElement("input");
            var label = document.createElement("label");
            li.id = "li" + jVariables[x].IdVariableCompuesta;
            input.type = "checkbox";
            input.setAttribute("class",li.id);
            input.setAttribute("onClick", "addActividadSelected('" + jVariables[x].IdVariableCompuesta + "')");
            label.innerHTML = jVariables[x].Descripcion;

            if (todasLasVariables[jVariables[x].IdVariableCompuesta].length > 0) {
                var span = document.createElement("span");
                span.setAttribute("class", "ui-icon ui-icon-triangle-1-e")
                span.id = "span" + jVariables[x].IdVariableCompuesta
                span.setAttribute("onclick", "expandeUL('" + jVariables[x].IdVariableCompuesta + "')");
                li.appendChild(span);
            }
            li.appendChild(input);
            li.appendChild(label);
            if (todasLasVariables[jVariables[x].IdVariableCompuesta].length > 0) {

                var ul = document.createElement("ul")
                ul.id = "ul" + jVariables[x].IdVariableCompuesta;
                ul.hidden = true;
                li.appendChild(ul);
            }


            if (jVariables[x].Nivel == 2) {
                comboDesglose.appendChild(li);
            } else {
                document.getElementById("ul" + jVariables[x].IdVariablePadre).appendChild(li);
            }
        }
    }


}


function escondeUl(ul) {
    document.getElementById("ul" + ul).hidden = true;
    document.getElementById("span" + ul).setAttribute("class", "ui-icon ui-icon-triangle-1-e");
    document.getElementById("span" + ul).setAttribute("onclick", "expandeUL('" + ul + "')");
}
function expandeUL(ul) {
    document.getElementById("ul" + ul).hidden = false;
    document.getElementById("span" + ul).setAttribute("class", "ui-icon ui-icon-triangle-1-se");
    document.getElementById("span" + ul).setAttribute("onclick", "escondeUl('" + ul + "')");

}

function addVariableSelected(variable) {
    varSelected[varSelected.length] = variable;
    varSelected.length++;

}
function addActividadSelected(variable,id) {
    //checar si esta seleccionado
    elemento = $('input.'+id);

    if (elemento.hasAttribute == "checked") {
        alert("ya tiene");
    }
    else {
      
        elemento.setAttribute("checked","t");
    }
}