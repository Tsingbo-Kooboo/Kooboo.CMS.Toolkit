!function(a){function t(){return!!this.points.length}function o(){var a=this;a.hasData()?a.hideNoData():a.showNoData()}var n=a.seriesTypes,e=a.Chart.prototype,i=a.getOptions(),l=a.extend;l(i.lang,{noData:"No data to display"}),i.noData={position:{x:0,y:0,align:"center",verticalAlign:"middle"},attr:{},style:{fontWeight:"bold",fontSize:"12px",color:"#60606a"}},n.pie.prototype.hasData=t,n.gauge&&(n.gauge.prototype.hasData=t),n.waterfall&&(n.waterfall.prototype.hasData=t),a.Series.prototype.hasData=function(){return void 0!==this.dataMax&&void 0!==this.dataMin},e.showNoData=function(a){var t=this,o=t.options,n=a||o.lang.noData,e=o.noData;t.noDataLabel||(t.noDataLabel=t.renderer.label(n,0,0,null,null,null,null,null,"no-data").attr(e.attr).css(e.style).add(),t.noDataLabel.align(l(t.noDataLabel.getBBox(),e.position),!1,"plotBox"))},e.hideNoData=function(){var a=this;a.noDataLabel&&(a.noDataLabel=a.noDataLabel.destroy())},e.hasData=function(){for(var a=this,t=a.series,o=t.length;o--;)if(t[o].hasData()&&!t[o].options.isInternal)return!0;return!1},e.callbacks.push(function(t){a.addEvent(t,"load",o),a.addEvent(t,"redraw",o)})}(Highcharts);