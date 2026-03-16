import { Component, inject, input, output } from '@angular/core';
import { RegisterCreds, User } from '../../../types/user';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../../core/services/account-service';
import { ToastService } from '../../../core/services/toast-service';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  private toastService = inject(ToastService);
  private accountService = inject(AccountService)
  public cancelRegister = output<boolean>();
  protected creds = {} as RegisterCreds;

  register() {
    this.accountService.register(this.creds).subscribe({
      next: (user) => {
        console.log(user);
        this.toastService.success('Registration successful');
        this.cancel();
      },
      error: (err) => {
        this.toastService.error('Failed to register');
        console.log(err);
      },
    });
  }

  cancel() {
    console.log('canceled!!!');
    this.cancelRegister.emit(false);

  }
}
