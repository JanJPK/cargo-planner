import React from 'react';
import { Nav, Navbar } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';

const Header = () => (
  <Navbar bg="dark" variant="dark">
    <LinkContainer to="/">
      <Navbar.Brand>Cargo Planner</Navbar.Brand>
    </LinkContainer>
    <Navbar.Toggle />
    <Navbar.Collapse>
      <LinkContainer to="/cargo-edit/new">
        <Nav.Link>New cargo</Nav.Link>
      </LinkContainer>
      <LinkContainer to="/cargo-list">
        <Nav.Link>Load cargo</Nav.Link>
      </LinkContainer>
      <LinkContainer to="/result-list">
        <Nav.Link>View results</Nav.Link>
      </LinkContainer>
    </Navbar.Collapse>
  </Navbar>
);

export default Header;
