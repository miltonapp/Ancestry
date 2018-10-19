//index.js
import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import ReactTable from 'react-table'
import 'react-table/react-table.css'
import {
	RadioGroup,
	RadioButton
}
from 'react-radio-buttons';

const apiUrl = 'http://localhost:58499/api/v1/person';

class AncestryUI extends React.Component {
	constructor(props) {
		super(props)
		this.state = {
			data: 'false',
			dataJson: [],
			userName: 'test',
			g1Checkbox: false,
			g2Checkbox: false,
			advancedSearch: false,
			Direction: 'Ancestors'
		};
		this._getData = this._getData.bind(this);
	}

	componentDidMount() {
		this._getData();
	}

	_getData = () => {
		/*         fetch(apiUrl)
		.then(response => {
		return response.json();
		})
		.then(json => this.setState({
		dataJson: json
		})) */
	}
	_searchUser(id, m, f) {
		// eslint-disable-next-line
		let url = apiUrl + '/' + id;

		if (this.state.advancedSearch) {
			url = url + '/' + this.state.Direction;
		}

		let gender = '';
		if (m) {
			gender = gender + 'M';
		}
		if (f) {
			gender = gender + 'F';
		}
		if (gender === '') {
			gender = 'MF';
		}

		url = url + '/gender/' + gender;
		fetch(url)
		.then(response => response.json())
		.then(json => {
			this.setState({
				dataJson: json
			})
		})
	}

	handleInputChange = (event) => {
		if (event === "Ancestors" || event === "Descendants") {
			this.setState({
				Direction: event
			});
		} else {
			const target = event.target;
			const value = target.type === 'checkbox' ? target.checked : target.value;
			const name = target.name;
			this.setState({
				[name]: value
			});
		}
	}

	toggle = () => {
		this.setState({
			advancedSearch: !this.state.advancedSearch
		});
	}

	render() {
		const columns = [{
				Header: 'ID',
				accessor: 'id', // String-based value accessors!
			}, {
				Header: 'NAME',
				accessor: 'name'
			}, {
				Header: 'GENDER',
				accessor: 'gender'
			}, {
				Header: 'BIRTHPLACE',
				accessor: 'birthPlace'
			}
		]

		let advancedGroup;
		let searchText = 'Search';
		let searchMode = 'Advanced Search';
		if (this.state.advancedSearch) {
			advancedGroup =  <><br/>
				<div className = "radioGroup"><label className = "direction">Direction:</label>
				 < RadioGroup onChange = {
				this.handleInputChange
			}
			name = "Direction" value = {
				this.state.Direction
			}
			horizontal >
			 < RadioButton value = "Ancestors" >
				Ancestors
				 </RadioButton>
				 <RadioButton value = "Descendants" >
				Descendants
				 </RadioButton>
				 </RadioGroup></div></>;
			searchText = 'Advanced Search';
			searchMode = 'Normal Search';
		}
		return (
			 <div className = "main">
			 <input placeholder = "Type a name to search"
				className = "searchInput"
				styles = "width:130px"
				name = "userName"
				onChange = {
				this.handleInputChange
			}
			/>
			 <button className = "search" onClick = {
				() => this._searchUser(this.state.userName, this.state.g1Checkbox, this.state.g2Checkbox)
			 }>{searchText}</button >
			 <br/>
			 <label  className = "gender"> Gender:  </label>

			 <input id = "maleBox" type = "checkbox" className="css-checkbox" onChange = {
				this.handleInputChange
			}
			name = "g1Checkbox" value = "M"/>
			 <label htmlFor="maleBox" className="css-label">
				Male
			 </label>
			 
			 <input id = "femaleBox" type = "checkbox" className = "css-checkbox" onChange = {
			this.handleInputChange
			}
			name = "g2Checkbox" value = "F"/>
			 <label htmlFor="femaleBox" className="css-label">
				Female
			 </label>
			 <h4 styles = "cursor:pointer" onClick = {
			() => this.toggle()
			}
			 >
			 {searchMode}
			 </h4> {advancedGroup}
		 <br/>
		 <label> Results:  </label>
		 <ReactTable className = "ReactTable"
		data = {
			this.state.dataJson
		}
		columns = {
			columns
		}
		defaultPageSize = {
			10
		}
		pageSizeOptions = {
			[10, 50]
		}
		/>
		</div > )
	}
}

ReactDOM.render( < AncestryUI /  > , document.getElementById('root'));