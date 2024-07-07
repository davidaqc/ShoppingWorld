import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NabvarAdminComponent } from './nabvar-admin.component';

describe('NabvarAdminComponent', () => {
  let component: NabvarAdminComponent;
  let fixture: ComponentFixture<NabvarAdminComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NabvarAdminComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NabvarAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
