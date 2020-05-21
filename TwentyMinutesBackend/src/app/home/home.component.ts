import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {

  backendData: {data: any, url: string};
  constructor() { }

  ngOnInit(): void {
  }


  onBackendCreated(event){
    this.backendData = {data: event[0], url: event[1]};
  }

}
