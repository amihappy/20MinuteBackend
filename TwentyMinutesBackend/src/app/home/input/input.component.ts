import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ValidateInputService } from './validateInput.service';
import { NotificationService } from 'src/app/notification.service';
import { BackendService } from 'src/app/backend.service';

@Component({
  selector: 'app-input',
  templateUrl: './input.component.html'
})
export class InputComponent implements OnInit {

  @Output() backendCreated = new EventEmitter();
  inputJson: string;

  constructor(
    private validateInputService: ValidateInputService,
    private notificationService: NotificationService,
    private backendService: BackendService) {
  }

  ngOnInit(): void {
  }


  onGenerateBackend(){
    if (this.validateInputService.ValidateJson(this.inputJson)){
        this.backendService.createNewBackendWithJson(this.inputJson).subscribe(data => {
          this.backendCreated.emit(data);
        });
    }
    else{
      this.notificationService.showError("Pasted text coudln't be parsed as an JSON", "Incorrect Json");
    }
  }

}
