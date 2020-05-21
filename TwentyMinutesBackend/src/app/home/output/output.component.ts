import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-output',
  templateUrl: './output.component.html'
})
export class OutputComponent implements OnInit {

  @Input() backendData: {data: any, url: string};

  constructor() { }

  ngOnInit(): void {
  }

}
