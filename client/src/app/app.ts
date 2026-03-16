import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs/internal/lastValueFrom';
import { Nav } from '../layout/nav/nav';
import { AccountService } from '../core/services/account-service';
import { Home } from "../feature/home/home";
import { User } from '../types/user';

@Component({
  selector: 'app-root',
  imports: [Nav, Home],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private accountService = inject(AccountService);
  private httpClient: HttpClient = inject(HttpClient);
  protected title = 'client';
  protected members = signal<User[]>([]);


  async ngOnInit() {
    this.members.set(await this.getMembers());
    this.setCurrentUser();
    // this.httpClient.get('https://localhost:5107/api/members').subscribe({
    //   next: (response) => {
    //     this.members.set(response);
    //     console.log(response);
    //   },
    //   error: (error) => console.log(error),
    //   complete: () => console.log('Request completed')
    // });
  }

  setCurrentUser() { 
    const userString = localStorage.getItem('user');
    if(!userString) return;
    const user = JSON.parse(userString);
    this.accountService.currentUser.set(user);  
  }

  async getMembers() {
    try {
      return lastValueFrom(this.httpClient.get<User[]>('https://localhost:5107/api/members'));
    } catch (error) {
      console.error('Error fetching members:', error);
      throw error;
    }
  }
}
