function generateChartInPercentage(zapAlerts) {

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
}

function generateChartInValues(zapAlerts) {

    am4core.useTheme(am4themes_material);
    am4core.useTheme(am4themes_animated);
    var chart = am4core.create("chartdiv", am4charts.PieChart3D);

    chart.data = zapAlerts;

    chart.legend = new am4charts.Legend();

    chart.innerRadius = am4core.percent(40);

    var pieSeries = chart.series.push(new am4charts.PieSeries());

    pieSeries.labels.template.text = "{category}: {value.value}";
    pieSeries.slices.template.tooltipText = "{category}: {value.value}";
    chart.legend.valueLabels.template.text = "{value.value}";

    pieSeries.dataFields.value = "alerts";
    pieSeries.dataFields.category = "risk";
    pieSeries.slices.template.stroke = am4core.color("#fff");
    pieSeries.slices.template.strokeOpacity = 1;

    pieSeries.hiddenState.properties.opacity = 1;
    pieSeries.hiddenState.properties.endAngle = -90;
    pieSeries.hiddenState.properties.startAngle = -90;
}

function generateBarChart(projectWiseVulnerabilities) {

    am4core.useTheme(am4themes_animated);

    // Create chart instance
    var chart = am4core.create("barChartDiv", am4charts.XYChart);

    chart.data = projectWiseVulnerabilities;

    // Create axes
    var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "project";
    categoryAxis.renderer.grid.template.location = 0;
    categoryAxis.renderer.minGridDistance = 30;    
    categoryAxis.adjustLabelPrecision = false;

    categoryAxis.renderer.labels.template.adapter.add("dy", function (dy, target) {
        if (target.dataItem && target.dataItem.index & 2 == 2) {
            return dy + 25;
        }
        return dy;
    });

    var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());

    // Create series
    var series = chart.series.push(new am4charts.ColumnSeries());
    series.dataFields.valueY = "issues";
    series.dataFields.categoryX = "project";
    series.name = "VulnerabilitiesCount";
    series.columns.template.tooltipText = "{categoryX}: [bold]{valueY}[/]";
    series.columns.template.fillOpacity = .8;

    var columnTemplate = series.columns.template;
    columnTemplate.strokeWidth = 2;
    columnTemplate.strokeOpacity = 1;
}