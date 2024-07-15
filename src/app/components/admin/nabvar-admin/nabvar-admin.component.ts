import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-nabvar-admin',
  templateUrl: './nabvar-admin.component.html',
  styleUrls: ['./nabvar-admin.component.css']
})
export class NabvarAdminComponent implements OnInit {

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
  }

  onLogout() {
    this.authService.logout();
    this.router.navigate(['/admin']);
  }

}
