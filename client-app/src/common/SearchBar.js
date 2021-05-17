import React from "react";
import { InputGroup, InputGroupAddon, Button, Input } from "reactstrap";

export default function SearchBar({ onSearch }) {
  const queryInput = React.useRef();

  function handleSubmit() {
    onSearch && onSearch(queryInput.current.value);
  }
  return (
    <>
      <InputGroup>
        <Input innerRef={queryInput} />
        <InputGroupAddon addonType="append">
          <Button color="secondary" onClick={handleSubmit}>
            <i className="fa fa-search" />
          </Button>
        </InputGroupAddon>
      </InputGroup>
    </>
  );
}
