function generateChart(zapAlerts) {

    am4core.useTheme(am4themes_material);
    am4core.useTheme(am4themes_animated);
    var chart = am4core.create("chartdiv", am4charts.PieChart3D);

    chart.data = zapAlerts;

    chart.legend = new am4charts.Legend();

    chart.innerRadius = am4core.percent(40);

    var pieSeries = chart.series.push(new am4charts.PieSeries());
    pieSeries.dataFields.value = "alerts";
    pieSeries.dataFields.category = "risk";
    pieSeries.slices.template.stroke = am4core.color("#fff");
    pieSeries.slices.template.strokeOpacity = 1;

    pieSeries.hiddenState.properties.opacity = 1;
    pieSeries.hiddenState.properties.endAngle = -90;
    pieSeries.hiddenState.properties.startAngle = -90;
    //chart.hiddenState.properties.radius = am4core.percent(0);

}