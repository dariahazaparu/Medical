import _React from 'react';
import _PropTypes from 'prop-types'; // @types/prop-types is a dependency of `@types/react`
import _Reactstrap from 'reactstrap'; // Third party library example

declare global {
    const React: typeof _React; // the React types _also_ exported by the React namespace, but export them again here just in case.
    const PropTypes: typeof _PropTypes;
    const Reactstrap: typeof _Reactstrap;
}
